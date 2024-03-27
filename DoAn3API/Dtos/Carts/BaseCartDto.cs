using AutoMapper;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.Carts
{
    [AutoMap(typeof(Cart))]
    public class BaseCartDto
    {
        public int UserId { get; set; }

        public string SessionId { get; set; }

        public string Token { get; set; }

        public string Status { get; set; }
    }
}
