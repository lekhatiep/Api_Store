using DoAn3API.Dtos.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Permissions
{
    public interface IPermissionService
    {
        Task<List<string>> GetAllPermissionByRoleId(int id);
        Task<List<string>> GetAllPermissionByRoleName(string name);
        Task<List<PermissionDto>> GetAllPermission();
        Task<int> AddNewPermission(string Name);
        Task DeletePermission(int Id);
        Task SyncNamePermission();
    }
}
