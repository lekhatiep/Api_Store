using AutoMapper;
using DoAn3API.Dtos.ProductImages;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using System.Linq;

namespace DoAn3API.Dtos.Products
{
    public class ProductAutoMapper : Profile
    {
        public ProductAutoMapper()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ImagePath, c => c.MapFrom(x => x.ProductImages != null ? x.ProductImages.Where(x => x.IsDefault == true).FirstOrDefault().ImagePath : string.Empty));
                ;
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductDto,UpdateProductDto>();

            CreateMap<PagedList<Product>, PagedList<ProductDto>>()
                .ConvertUsing<ProductPagedListTypeConverter<Product, ProductDto>>();

           
        }
    }
}
