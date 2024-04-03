using Domain.Entities.Catalog;
using Infastructure.Data;

namespace Infastructure.Repositories.Catalogs.OrderItemRepos
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
