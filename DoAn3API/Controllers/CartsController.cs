using DoAn3API.Authorize.CustomAuthorize;
using DoAn3API.Dtos.CartItems;
using DoAn3API.Services.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn3API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartsController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/<CartsController>
        [CustomAuthorize(Constants.NamePermissions.Carts.View)]
        [HttpGet("GetListCart")]
        public async Task<IActionResult> GetListCart()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userId = int.Parse(identity.FindFirst("Id").Value);

            var cart = await _cartService.GetCartUserById(userId);

            if (cart == null)
            {
                return Ok(new List<CartItemDto>());
            }

            var listCart = await _cartService.GetUserListCartItem(cart.Id);
            return Ok(listCart?? new List<CartItemDto>());
            


        }

        // GET api/<CartsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CartsController>
        [CustomAuthorize(Constants.NamePermissions.Carts.Create)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCartItemDto createCartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userId = int.Parse(identity.FindFirst("Id").Value);

            await _cartService.AddToCart(createCartItemDto);
            var cart = await _cartService.GetCartUserById(userId); //Admin
            var listCart = await _cartService.GetUserListCartItem(cart.Id);

            return Ok(listCart);
        }

        [CustomAuthorize(Constants.NamePermissions.Carts.Edit)]
        [HttpPost("UpdateOrRemoveCartItem")]
        public async Task<IActionResult> UpdateOrRemoveCartItem( [FromBody] List<UpdateCartItemDto> cartItemDtos)
        {
            try
            {
                var listCartItem = await _cartService.UpdateOrRemoveCartItem(cartItemDtos);
                return Ok(listCartItem);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [CustomAuthorize(Constants.NamePermissions.Carts.Edit)]
        [HttpPost("UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateCartItemDto cartItemDtos)
        {
            try
            {
                await _cartService.UpdateItem(cartItemDtos);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // DELETE api/<CartsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [CustomAuthorize(Constants.NamePermissions.Carts.View)]
        // GET: api/<CartsController>
        [HttpGet("GetListCartItemChecked")]
        public async Task<IActionResult> GetListCartItemChecked()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userId = int.Parse(identity.FindFirst("Id").Value);

            var cart = await _cartService.GetCartUserById(userId); //Admin
            if (cart == null)
            {
                return NotFound();
            }
            var listCart = await _cartService.GetUserListCartItemChecked(cart.Id);

            return Ok(listCart);
        }
    }
}
