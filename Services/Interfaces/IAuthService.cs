using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IAuthService
{
    public Task<TokenDto> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<TokenDto> LoginUser(LoginCredentials loginCredentials);
    public Task<LoggedOutDto> LogoutUser(HttpContext httpContext);
}