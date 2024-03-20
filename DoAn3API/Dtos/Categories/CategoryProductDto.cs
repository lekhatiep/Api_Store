using AutoMapper;
using DoAn3API.Dtos.ProductImages;
using DoAn3API.Dtos.Products;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.Categories
{
    [AutoMap(typeof(Category))]
    public class CategoryProductDto
    {
        public CategoryProductDto()
        {
            
        }

        public CategoryProductDto(Category c, ProductDto p, ProductImageDto i)
        {
            Id = c.Id;
            Name = c.Name;
            Product = p;
            //Product.ProductImages = new List<ProductImageDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ProductDto Product { get; set; }
    }
}
