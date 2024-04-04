using DoAn3API.Authorize.CustomAuthorize;
using DoAn3API.Constants;
using DoAn3API.Dtos.OrderItems;
using DoAn3API.Dtos.Orders;
using DoAn3API.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn3API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: api/<OrdersController>
        [CustomAuthorize(NamePermissions.Orders.View)]
        [HttpGet("HistoryOrderByUser")]
        public async Task<IActionResult> HistoryOrderByUser(string status)
        {
            try
            {
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userId = int.Parse(identity.FindFirst("Id").Value);
                var listOrderItem = await _orderService.GetListHistoryOrderByUser(userId, status);//admin
                return Ok(listOrderItem?? new List<OrderItemDto>());
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // POST api/<OrdersController>
        [CustomAuthorize(NamePermissions.Orders.Create)]
        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] CreateOrderDto create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _orderService.ProcessCheckoutOrder(create.UserId);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
