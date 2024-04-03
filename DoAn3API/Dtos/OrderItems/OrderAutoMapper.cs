using AutoMapper;
using DoAn3API.Dtos.Orders;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.OrderItems
{
    public class OrderAutoMapper : Profile
    {
        public OrderAutoMapper()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<OrderItemDto, OrderItem>();
        }
    }
}
