using AutoMapper;
using DoAn3API.Authorize.CustomAuthorize;
using DoAn3API.Constants;
using DoAn3API.Dtos.Identity;
using DoAn3API.Helper;
using DoAn3API.Services.Authenticate;
using DoAn3API.Services.Users;
using Domain.Common.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoAn3API.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _authenticateService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;
        public UsersController(
            IMapper mapper, 
            IAuthenticateService authenticateService,
            IUserService userService,
            IHttpContextAccessor httpContext)
        {
            _mapper = mapper;
            _authenticateService = authenticateService;
            _userService = userService;
            _httpContext = httpContext;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rs = await _authenticateService.Register(user);
                if (rs != -1)
                {
                    return Ok();

                }
                else
                {
                   return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authenticateService.Authenticate(login);

                if (result == null)
                    return BadRequest();
                return Ok(result);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }           
        }

        [CustomAuthorize(NamePermissions.User.View)]
        [HttpGet]
        [Route("GetInfo")]
        public async Task<IActionResult> GetInfoUser()
        {
            try
            {
                var identity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
                var id = int.Parse(identity.FindFirst("id").Value);
                var user = await _userService.GetUserById(id);

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<string>(e.Message));
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validateToken")]
        public IActionResult ValidateToken([FromHeader] string accessToken)
        {

            try
            {
                var isValid = JwtToken.ValidateToken(accessToken);
                if (!isValid)
                    return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<string>(e.Message));
            }

        }

        [CustomAuthorize(NamePermissions.User.View)]
        [Route("GetUserPermission/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserPermission(int userId)
        {

            try
            {
                var permissions = await _userService.GetAllPermissionByUserId(userId);

                return Ok(permissions);
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<string>(e.Message));
            }

        }

        [Route("CheckEmailExists")]
        [HttpGet]
        public async Task<IActionResult> CheckEmailExists(string email)
        {

            try
            {
                var tempCreateUserDto = new CreateUserDto()
                {
                    Email = email,
                    Password = string.Empty,
                    Phone = string.Empty,
                    UserName = string.Empty

                };
                var rs = await _userService.IsExistsUser(tempCreateUserDto);

                return Ok(rs);

            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<string>(e.Message));
            }

        }
    }
}
