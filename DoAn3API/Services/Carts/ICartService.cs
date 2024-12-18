using DoAn3API.Dtos.CartItems;
using DoAn3API.Dtos.Carts;
using Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn3API.Services.Carts
{
    public interface ICartService
    {
        Task<List<CartItemDto>> AddToCart(CreateCartItemDto createCartItem);

        Task<int> CreateNewCart(CreateCartDto createCartDto);

        Task<int> CheckUserExistCart(int cartId);

        Task<List<CartItemDto>> GetUserListCartItem(int cartId);

        Task<Cart> GetCartUserById(int userId);


        Task<List<CartItemDto>> UpdateOrRemoveCartItem(List<UpdateCartItemDto>  updateCartItemDtos);

        Task<bool> UpdateItem(UpdateCartItemDto updateCartItemDto);

        Task<List<CartItemDto>> GetUserListCartItemChecked(int cartId);

        Task<List<CartItemDto>> GetUserListCartItemIsOrder(int cartId);

        Task SyncListCartCartItem(List<CartItemDto> listCartItem);

        Task<Cart> GetCurrentCartIDByUser();
        Task<int> UpdateItemInCart(UpdateCartItemDto updateCartItemDto);

    }
}
