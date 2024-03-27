using Domain.Entities.Catalog;
using Infastructure.Data;

namespace Infastructure.Repositories.Catalogs.CartItemRepos
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
