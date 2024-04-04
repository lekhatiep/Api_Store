using AutoMapper;
using Domain.Entities.Catalog;
using System;

namespace DoAn3API.Dtos.OrderItems
{
    [AutoMap(typeof(OrderItem))]
    public class BaseOrderItemDto
    {
        public int ProductId { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        public double Quantity { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
