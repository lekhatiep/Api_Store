using AutoMapper;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Dtos.Products
{
    public class ProductPagedListTypeConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>> 
       where TSource : Product
       where TDestination : ProductDto
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var result = new List<TDestination>();

            foreach (var item in source)
            {
                var productDto = context.Mapper.Map<TSource, TDestination>(item);
                if (item.ProductCategories.Any())
                {
                    productDto.CategoryId = item.ProductCategories.FirstOrDefault().CategoryId; 
                }
                
                result.Add(productDto);
            }

            return new PagedList<TDestination>(result, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
