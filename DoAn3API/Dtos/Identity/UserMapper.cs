using AutoMapper;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Dtos.Identity
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UserRoleDto, UserRole>();
            CreateMap<RoleDto, Role>();
            CreateMap<UserDto, User>();
        }
    }
}
