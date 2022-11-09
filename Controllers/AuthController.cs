using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public async Task<TokenDto> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return await _authService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ValidateToken")]
    [Route("logout")]
    public async Task<LoggedOutDto> Logout()
    {
        return await _authService.LogoutUser(HttpContext);
    }

    [HttpPost]
    [Route("login")]
    public async Task<TokenDto> Login([FromBody] LoginCredentials loginCredentials)
    {
        return await _authService.LoginUser(loginCredentials);
    }
}