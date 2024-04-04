using DoAn3API.Dtos.OrderItems;
using DoAn3API.Dtos.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn3API.Services.Orders
{
    public interface IOrderService
    {
        Task<int> Checkout(CreateOrderDto createOrderDto);
        Task<List<OrderItemDto>> GetListHistoryOrderByUser(int userId, string status);
        Task<int> ProcessCheckoutOrder(int userId);
        Task<int> CancelOrder(int orderId);
    }
}
