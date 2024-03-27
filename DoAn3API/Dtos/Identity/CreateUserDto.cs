using AutoMapper;
using Domain.Entities.Identity;

namespace DoAn3API.Dtos.Identity
{
    [AutoMap(typeof(User))]
    public class CreateUserDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
