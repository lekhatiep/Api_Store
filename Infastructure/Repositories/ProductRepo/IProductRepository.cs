using Domain.Common.Paging;
using Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Repositories.ProductRepo
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //Custom new method here
       // PagedList<Product> GetProductPage(PagedRequestBase pagedRequest);

        Task<int> AddProductReturnId(Product product);

        Task DeleteProduct(int id);
    }
}
