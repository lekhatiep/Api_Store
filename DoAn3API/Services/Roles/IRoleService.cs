using DoAn3API.Dtos.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn3API.Services.Roles
{
    public interface IRoleService
    {
        Task AssignRoleDefault(string roleName, int userId);
        Task<List<RoleDto>> ListRole();
        Task AddPermissionToRole(int roleID, List<int> permissionID);
        Task DeleteRole(int roleID);
    }
}
