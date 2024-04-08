using AutoMapper;
using DoAn3API.Helper;
using Domain.Common.Paging;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.Orders
{
    public class OrderAutoMapper : Profile
    {
        public OrderAutoMapper()
        {
            CreateMap<PagedList<Order>, PagedList<OrderDto>>()

                .ConvertUsing<ListOrderPagedListTypeConverterITypeConverter<Order, OrderDto>>();


        }
    }
}
