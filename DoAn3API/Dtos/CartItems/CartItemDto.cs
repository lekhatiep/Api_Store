using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Dtos.CartItems
{
    public class CartItemDto : BaseCartItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImgPath { get; set; }

        public double Total { get; set ; }
    }
}
