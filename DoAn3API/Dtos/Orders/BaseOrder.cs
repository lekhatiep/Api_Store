using AutoMapper;
using DoAn3API.Dtos.OrderItems;
using Domain.Entities.Catalog;
using System.Collections.Generic;

namespace DoAn3API.Dtos.Orders
{
    [AutoMap(typeof(Order))]
    public class BaseOrder
    {
        private IList<OrderItemDto> _orderItems;

        public int UserId { get; set; }

        public string SessionId { get; set; }

        public string Token { get; set; }

        public string Status { get; set; }

        public double SubTotal { get; set; }

        public double ItemDisCount { get; set; }

        public double Tax { get; set; }

        public double Shipping { get; set; }

        public double Total { get; set; }

        public double Promo { get; set; }

        public double Discount { get; set; }

        public double GrandTotal { get; set; }

        public IList<OrderItemDto> OrderItems
        {
            get => _orderItems ?? new List<OrderItemDto>();
            set => _orderItems = value;
        }
    }
}
