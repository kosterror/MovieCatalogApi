using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/account")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public TokenDto RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return _authService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    [Route("logout")]
    public LoggedOutDto Logout()
    {
        return _authService.LogoutUser(HttpContext);
    }

    [HttpPost]
    [Route("login")]
    public async Task<TokenDto> Login([FromBody] LoginCredentials loginCredentials)
    {
        return _authService.LoginUser(loginCredentials);
    }
}