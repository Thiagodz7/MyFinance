using MyFinance.Web.DTOs.Auth;

namespace MyFinance.Web.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> Login(LoginUserDto loginModel);
        Task Logout();
        Task<bool> Register(RegisterUserDto registerModel);
    }
}
