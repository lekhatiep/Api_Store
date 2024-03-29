using Domain.Entities.Identity;
using Infastructure.Data;

namespace Infastructure.Repositories.PermissionRepo
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {

        public PermissionRepository(AppDbContext context) : base(context)
        {

        }
    }
}
