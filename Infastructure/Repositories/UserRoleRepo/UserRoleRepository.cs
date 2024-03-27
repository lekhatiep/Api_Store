using Domain.Entities.Identity;
using Infastructure.Data;

namespace Infastructure.Repositories.UserRoleRepo
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
