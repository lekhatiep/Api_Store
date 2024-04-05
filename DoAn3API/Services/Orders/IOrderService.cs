using DoAn3API.Dtos.OrderItems;
using DoAn3API.Dtos.Orders;
using Domain.Common.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn3API.Services.Orders
{
    public interface IOrderService
    {
        Task<int> Checkout(CreateOrderDto createOrderDto);
        Task<List<OrderItemDto>> GetListHistoryOrderByUser(int userId, int status);
        Task<int> ProcessCheckoutOrder(int userId);
        Task<int> CancelOrder(int orderId);
        PagedList<OrderDto> GetListOderPaging(PagedOrderRequestDto pagedOrderRequest);
        Task<List<OrderItemDto>> GetDetailOrder(int orderID);
        Task<int> UpdateStatusOrder(int orderID, int status);
    }
}
