using AutoMapper;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using System.Collections.Generic;

namespace DoAn3API.Dtos.Orders
{
    public class ListOrderPagedListTypeConverterITypeConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
       where TSource : Order
       where TDestination : OrderDto
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
    {
        var result = new List<TDestination>();

        foreach (var item in source)
        {
            var ordertDto = context.Mapper.Map<TSource, TDestination>(item);
            if (item.User != null)
            {
                ordertDto.UserName = item.User.UserName;
            }

            result.Add(ordertDto);
        }

        return new PagedList<TDestination>(result, source.TotalCount, source.CurrentPage, source.PageSize);
    }
}
}
