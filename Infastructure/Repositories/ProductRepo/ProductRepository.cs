using Domain.Common.Paging;
using Domain.Entities.Catalog;
using Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repositories.ProductRepo
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> AddProductReturnId(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;

        }

        public  async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            product.IsDelete = true;
            await _context.SaveChangesAsync();
        }

        //public  PagedList<Product> GetProductPage(PagedRequestBase pagedRequest)
        //{
        //    var productQuery = this.List();

        //    return PagedList<Product>.ToPagedList(productQuery, pagedRequest.PageNumber, pagedRequest.PageSize);
        //}
    }
}
