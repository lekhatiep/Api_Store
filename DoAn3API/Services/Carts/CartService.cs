using AutoMapper;
using DoAn3API.Constants.Catalogs;
using DoAn3API.Dtos.CartItems;
using DoAn3API.Dtos.Carts;
using DoAn3API.Services.Users;
using Domain.Entities.Catalog;
using Infastructure.Repositories.Catalogs.CartItemRepos;
using Infastructure.Repositories.Catalogs.CartRepos;
using Infastructure.Repositories.Catalogs.ProductCategoryRepo;
using Infastructure.Repositories.ProductRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Carts
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper _mapper;

        public CartService(
            IMapper mapper,
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _userService = userService;
            this.httpContextAccessor = httpContextAccessor;
        }
 

        public async Task<List<CartItemDto>> AddToCart(CreateCartItemDto createCartItem)
        {
            var cartId = await CheckUserExistCart(createCartItem.UserId);

            if (cartId != -1)
            {
                var listCart = await GetUserListCartItem(cartId);
                var itemExists = listCart.Where(x => x.ProductId == createCartItem.ProductId).FirstOrDefault();
                if (itemExists != null)
                {
                    var carItemExists = await _cartItemRepository.List().Where(x => x.Id == itemExists.Id).FirstOrDefaultAsync();

                    carItemExists.Quantity+= createCartItem.Quantity;
                    await _cartItemRepository.Update(carItemExists, carItemExists.Id);

                }
                else
                {
                    createCartItem.CartId = cartId;

                    await _cartItemRepository.Insert(_mapper.Map<CartItem>(createCartItem));
                    await _cartItemRepository.Save();
                }
            }
            else
            {
                var newCart = new Cart()
                {
                    UserId = createCartItem.UserId,
                    Status = CatalogConst.CartStatus.PENDING,
                };

                await _cartRepository.Insert(newCart);
                await _cartRepository.Save();

                createCartItem.CartId = newCart.Id;
                cartId = newCart.Id;

                await _cartItemRepository.Insert(_mapper.Map<CartItem>(createCartItem));
                await _cartItemRepository.Save();
                
            }

            return await GetUserListCartItem(cartId);
        }

        public async Task<int> CreateNewCart(CreateCartDto createCartDto)
        {

            var cartId = await CheckUserExistCart(createCartDto.UserId);

            if (cartId == -1)
            {
                return -1;
            }

            createCartDto.Status = CatalogConst.CartStatus.PENDING;

            var newCart = _mapper.Map<Cart>(createCartDto);
            await _cartRepository.Insert(newCart);
            await _cartRepository.Save();

            return newCart.Id;
        }
      

        public async Task<int> CheckUserExistCart(int userId)
        {
            var cart = await _cartRepository.List().Where(x => x.UserId == userId && x.Status.Contains(CatalogConst.CartStatus.PENDING)).FirstOrDefaultAsync();

            if (cart == null)
            {
                return -1;
            }

            return cart.Id;
        }

        public async Task<List<CartItemDto>> GetUserListCartItem(int cartId)
        {

            var queryCart = await (from c in _cartRepository.List()
                             join ci in _cartItemRepository.List() on c.Id equals ci.CartId into cic
                             from ci in cic.DefaultIfEmpty()
                             join p in _productRepository.List()
                                         .Include(x => x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete)) on ci.ProductId equals p.Id
                             where c.Id == cartId && c.Status.Contains(CatalogConst.CartStatus.PENDING) && c.IsDelete == false && p.IsDelete == false

                             select new { ci, p }
                             )

                             .Select(x => new CartItemDto
                             {
                                 Id = x.ci.Id,
                                 CartId = x.ci.CartId,
                                 ProductId = x.p.Id,
                                 ImgPath = x.p.ProductImages.FirstOrDefault().ImagePath,
                                 Title = x.p.Title,
                                 Price = x.ci.Price,
                                 Quantity = x.ci.Quantity,
                                 Total = Convert.ToDouble(x.ci.Quantity * x.p.Price),
                                 Active = x.ci.Active
                             }).ToListAsync();
                            ;

            return queryCart;
           
        }

        public async Task<Cart> GetCartUserById(int userId)
        {
            var cart = await _cartRepository.List().Where(x => x.UserId == userId && x.Status.Contains(CatalogConst.CartStatus.PENDING) && x.IsDelete == false).FirstOrDefaultAsync();

            return cart;
        }

        public async Task<List<CartItemDto>> UpdateOrRemoveCartItem(List<UpdateCartItemDto> updateCartItemDtos)
        {
            var userId = (int)httpContextAccessor.HttpContext.Items["Id"];
            var cart = await GetCartUserById(userId);
            var listCartItems = await _cartItemRepository.List().Where(x => x.CartId == cart.Id).ToListAsync();

            if (updateCartItemDtos.Any())
            {
                //Remove/Update cart item
                foreach (var cartItem in listCartItems)
                {
                    if (updateCartItemDtos.Any(x => x.Id == cartItem.Id))
                    {
                        var existsItem = updateCartItemDtos.FirstOrDefault(x => x.Id == cartItem.Id);

                        await _cartItemRepository.Update(_mapper.Map<CartItem>(existsItem), cartItem.Id);
                    }
                    else
                    {
                            
                        await _cartItemRepository.Delete(cartItem.Id);
                        await _cartItemRepository.Save();
                    }
                }

                //Add new to cart item
                //foreach (var cartItem in updateCartItemDtos)
                //{
                //    if (listCartItems.Any(x => x.Id == cartItem.Id))
                //    {
                //        var existsItem = updateCartItemDtos.FirstOrDefault(x => x.Id == cartItem.Id);

                //        await _cartItemRepository.Update(_mapper.Map<CartItem>(existsItem), cartItem.Id);
                //    }
                //    else
                //    {
                //        updateCartItemDtos.Remove(cartItem);
                //        await _cartItemRepository.Delete(cartItem.Id);
                //    }
                //}
                
            }
            else
            {
                if (listCartItems.Any())
                {
                    foreach (var cartItem in listCartItems)
                    {
                        await _cartItemRepository.Delete(cartItem.Id);
                        await _cartItemRepository.Save();
                    }
                }
               
            }


            return await GetUserListCartItem(cart.Id);
        }

        public async Task<bool> UpdateItem(UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _cartItemRepository.List().Where(x => x.Id == updateCartItemDto.Id).FirstOrDefaultAsync();
            var product = await _productRepository.List().Where(x => x.Id == cartItem.ProductId).FirstOrDefaultAsync();

            cartItem.Quantity = updateCartItemDto.Quantity;
            cartItem.Active = updateCartItemDto.Active;
            cartItem.IsChecked = updateCartItemDto.IsChecked;
            cartItem.IsOrder = updateCartItemDto.IsOrder;
           
          

            await _cartItemRepository.Update(cartItem, updateCartItemDto.Id);

            return true;

        }

        public async Task<List<CartItemDto>> GetUserListCartItemChecked(int cartId)
        {
            var queryCart = await(from c in _cartRepository.List()
                                  join ci in _cartItemRepository.List() on c.Id equals ci.CartId into cic
                                  from ci in cic.DefaultIfEmpty()
                                  join p in _productRepository.List()
                                              .Include(x => x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete)) on ci.ProductId equals p.Id
                                  where c.Id == cartId && c.Status.Contains(CatalogConst.CartStatus.PENDING) && c.IsDelete == false && p.IsDelete == false
                                  && ci.Active == true

                                  select new { ci, p }
                             )

                             .Select(x => new CartItemDto
                             {
                                 Id = x.ci.Id,
                                 CartId = x.ci.CartId,
                                 ProductId = x.p.Id,
                                 ImgPath = x.p.ProductImages.FirstOrDefault().ImagePath,
                                 Title = x.p.Title,
                                 Price = x.ci.Price,
                                 Quantity = x.ci.Quantity,
                                 Total = Convert.ToDouble(x.ci.Quantity * x.p.Price),
                                 Active = x.ci.Active
                             }).ToListAsync();

            return queryCart;
        }

        public async Task<List<CartItemDto>> GetUserListCartItemIsOrder(int cartId)
        {
            var queryCart = await (from c in _cartRepository.List()
                                   join ci in _cartItemRepository.List() on c.Id equals ci.CartId into cic
                                   from ci in cic.DefaultIfEmpty()
                                   join p in _productRepository.List()
                                               .Include(x => x.ProductImages.Where(x => x.IsDefault == true && x.IsDelete)) on ci.ProductId equals p.Id
                                   where c.Id == cartId && c.Status.Contains(CatalogConst.CartStatus.PENDING) && c.IsDelete == false && p.IsDelete == false
                                   && ci.Active == true

                                   select new { ci, p }
                             )

                             .Select(x => new CartItemDto
                             {
                                 Id = x.ci.Id,
                                 CartId = x.ci.CartId,
                                 ProductId = x.p.Id,
                                 ImgPath = x.p.ProductImages.FirstOrDefault().ImagePath,
                                 Title = x.p.Title,
                                 Price = x.ci.Price,
                                 Quantity = x.ci.Quantity,
                                 Total = Convert.ToDouble(x.ci.Quantity * x.p.Price),
                                 Active = x.ci.Active,
                                 IsOrder = x.ci.IsOrder
                             }).ToListAsync();

            return queryCart;
        }
    }
}
