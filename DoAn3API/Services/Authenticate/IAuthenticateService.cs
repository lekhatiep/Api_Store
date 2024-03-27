using DoAn3API.Dtos.Identity;
using DoAn3API.Dtos.TokenDto;
using System.Threading.Tasks;

namespace DoAn3API.Services.Authenticate
{
    public interface IAuthenticateService
    {
        Task<AuthReponseDto> Authenticate(LoginDto loginDto);
        Task<int> Register(CreateUserDto loginDto);
    }
}
