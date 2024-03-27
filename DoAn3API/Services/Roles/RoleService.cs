using Domain.Entities.Identity;
using Infastructure.Repositories.RoleRepo;
using Infastructure.Repositories.UserRoleRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository
            )
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task AssignRoleDefault(string roleName, int userId)
        {
            var role = await _roleRepository.List().Where(x => x.Name.ToLower().Contains(roleName.ToLower().Trim())).FirstOrDefaultAsync();

            if(role == null)
            {
                return;
            }


            var userRole = new UserRole() {
                RoleId = role.Id,
                UserId = userId
            };

            await _userRoleRepository.Insert(userRole);
            await _userRoleRepository.Save();
        }
    }
}
