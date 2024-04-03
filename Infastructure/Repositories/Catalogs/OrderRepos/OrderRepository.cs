using Domain.Entities.Catalog;
using Infastructure.Data;

namespace Infastructure.Repositories.Catalogs.OrderRepos
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }


    }
}
