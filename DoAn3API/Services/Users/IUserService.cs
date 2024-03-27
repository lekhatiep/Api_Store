using DoAn3API.Dtos.Identity;
using Domain.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn3API.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<List<string>> GetAllPermissionByUserId(int id);
        Task<bool> IsExistsUser(CreateUserDto createUserDto);
    }
}
