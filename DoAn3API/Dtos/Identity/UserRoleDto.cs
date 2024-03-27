using AutoMapper;
using Domain.Entities.Identity;

namespace DoAn3API.Dtos.Identity
{
    [AutoMap(typeof(UserRole))]
    public class UserRoleDto
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
