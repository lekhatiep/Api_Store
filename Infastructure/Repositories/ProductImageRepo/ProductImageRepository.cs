using Domain.Entities.Catalog;
using Infastructure.Data;

namespace Infastructure.Repositories.ProductImageRepo
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
