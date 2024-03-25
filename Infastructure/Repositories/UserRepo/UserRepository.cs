using Domain.Entities.Identity;
using Infastructure.Data;

namespace Infastructure.Repositories.UserRepo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
