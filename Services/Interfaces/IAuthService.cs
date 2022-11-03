using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IAuthService
{
    public TokenDto RegisterUser(UserRegisterDto userRegisterDto);
    public TokenDto LoginUser(LoginCredentials loginCredentials);
    public LoggedOutDto LogoutUser(HttpContext httpContext);
}