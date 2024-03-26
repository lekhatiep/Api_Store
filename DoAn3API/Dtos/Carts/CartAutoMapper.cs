using AutoMapper;
using DoAn3API.Dtos.CartItems;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.Carts
{
    public class CartAutoMapper : Profile
    {
        public CartAutoMapper()
        {
            CreateMap<CreateCartDto, Cart>();
            CreateMap<CreateCartItemDto, CartItem>();
            CreateMap<UpdateCartItemDto, CartItem>();
        }
    }
}
