using AutoMapper;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.CartItems
{
    [AutoMap(typeof(CartItem))]
    public class BaseCartItemDto
    {
        public int ProductId { get; set; }

        public int CartId { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        public int Quantity { get; set; }

        public bool Active { get; set; }

        public bool IsChecked { get; set; }

        public bool IsOrder { get; set; }
    }
}
