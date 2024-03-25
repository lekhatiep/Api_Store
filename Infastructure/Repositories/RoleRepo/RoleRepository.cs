using Domain.Entities.Identity;
using Infastructure.Data;

namespace Infastructure.Repositories.RoleRepo
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {

        public RoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
