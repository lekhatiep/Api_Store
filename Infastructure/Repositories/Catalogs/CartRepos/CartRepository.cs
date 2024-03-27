using Domain.Entities.Catalog;
using Infastructure.Data;

namespace Infastructure.Repositories.Catalogs.CartRepos
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }


    }
}
