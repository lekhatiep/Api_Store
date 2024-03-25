using DoAn3API.Dtos.Identity;
using Domain.Entities.Identity;
using Infastructure.Data;
using Infastructure.Repositories.UserRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public UserService(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<List<string>> GetAllPermissionByUserId(int id)
        {
            var userQuery = _context.Users;
            var roleQuery = _context.Role;
            var permissionQuery = _context.Permissions;

            var role = from r in roleQuery
                       join ur in _context.UserRoles on r.Id equals ur.RoleId
                       join u in userQuery on ur.UserId equals u.Id
                       where u.Id == id
                       select r;

            if (role is null)
            {
                return null;
            }

            var rolePermission = from r in role
                                 join rp in _context.RolePermissions on r.Id equals rp.RoleId
                                 select rp;

            var permission = await (from rp in rolePermission
                                    join p in permissionQuery on rp.PermissionId equals p.Id
                              select p).ToListAsync();

            var listPermission = permission.Select(x => x.Name).ToList();

            return listPermission;
        }

        public async Task<User> GetUserById(int id)
        {
            var userQuery = _context.Users;
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            return user;
        }


        public async Task<bool> IsExistsUser(CreateUserDto createUserDto)
        {
            createUserDto.Password = createUserDto.Password ?? string.Empty;
            createUserDto.UserName = createUserDto.UserName ?? string.Empty;
            createUserDto.Phone = createUserDto.UserName ?? string.Empty;

            var user = await _userRepository.List()
                .Where(x => x.Email.ToLower() == createUserDto.Email.ToLower().Trim()).FirstOrDefaultAsync();

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}
