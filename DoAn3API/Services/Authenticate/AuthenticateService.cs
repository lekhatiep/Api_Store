using AutoMapper;
using DoAn3API.Dtos.Identity;
using DoAn3API.Dtos.TokenDto;
using DoAn3API.Helper;
using DoAn3API.Services.Roles;
using DoAn3API.Services.Users;
using Domain.Entities.Identity;
using Infastructure.Data;
using Infastructure.Repositories.UserRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DoAn3API.Services.Authenticate
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AuthenticateService(
            IUserRepository userRepository,
            AppDbContext context,
             IMapper mapper,
             IUserService userService,
             IRoleService roleService
            )
        {
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }
        public async Task<AuthReponseDto> Authenticate(LoginDto login)
        {
            string SECRET_KEY = "KeyOfMyshop10121994"; // in appsettings.json
            SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
            //var queryUser = await _context.Users.ToListAsync();
            var queryRole = _context.Role;
            var queryRolePermission = _context.RolePermissions;
            var queryPermission = _context.Permissions;

            var user = await _context.Users
                      .Include(x => x.UserRoles)
                      .ThenInclude(x => x.Role)
                      .Where( x => x.Email.ToLower() == login.Email.ToLower())
                      .Where( x => x.Password.ToLower() == login.Password.ToLower())
                      .SingleOrDefaultAsync();

            if (user is null)
            {
                return null;
            }


            //Also note that sercurity length should be > 256b
            //Tao chung chi + kieu ma hoa cho chu ky 
            var credentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            //Header of JWT
            var header = new JwtHeader(credentials);

            //Token will be good for 1 minutes + refresh_token

            DateTime Expriry = DateTime.UtcNow.AddMinutes(9000);
            int ts = (int)(Expriry - new DateTime(1970, 1, 1)).TotalSeconds;
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("id",user.Id.ToString()));

            var permisions = string.Empty;

            var listPermissionName = new List<string>();

            var secToken = new JwtSecurityToken(
                AppSettings.ISSUER,
                AppSettings.AUDIENCE,
                claims,
                expires: DateTime.UtcNow.AddMinutes(9000),
                signingCredentials: credentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);//Sercurity Token

            return new AuthReponseDto
            {
                Token = tokenString,
                TokenType = "Bearer",
                TotalSecond = ts
            };
        }

        public async Task<int> Register(CreateUserDto loginDto)
        {
            var existsUser = await _userService.IsExistsUser(loginDto);

            if (existsUser)
            {
                return -1;
            }

            var newUser = _mapper.Map<User>(loginDto);

           // var userName = newUser.Email.Substring(newUser.Email.IndexOf('@'), newUser.Email.Length);
            newUser.UserName = newUser.Email;

            await _userRepository.Insert(newUser);
            await _userRepository.Save();

            await _roleService.AssignRoleDefault("member", newUser.Id);


            return newUser.Id;
        }
    }
}
