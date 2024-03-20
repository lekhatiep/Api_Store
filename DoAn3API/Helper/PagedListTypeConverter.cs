using AutoMapper;
using Domain.Common.Paging;
using System.Linq;

namespace DoAn3API.Helper
{
    public class PagedListTypeConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
       where TSource : class
       where TDestination : class
    {

        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var vm = source.Select(m => context.Mapper.Map<TSource, TDestination>(m)).ToList();

            return new PagedList<TDestination>(vm, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
