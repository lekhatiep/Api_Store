using System.Threading.Tasks;

namespace DoAn3API.Services.Roles
{
    public interface IRoleService
    {
        Task AssignRoleDefault(string roleName, int userId);
        Task ListRole();
    }
}
