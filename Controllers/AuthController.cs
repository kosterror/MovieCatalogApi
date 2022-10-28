using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/account")]
[ApiController]
public class AuthController : ControllerBase
{
    private IAuthService _authService;
    private IValidateTokenService _validateTokenService;

    public AuthController(IAuthService authService, IValidateTokenService validateTokenService)
    {
        _authService = authService;
        _validateTokenService = validateTokenService;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return await _authService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Authorize]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        //нельзя давать возможность два раза ралогиниться под одним и тем же токеном
        if (await _validateTokenService.IsValidToken(HttpContext.Request.Headers))
        {
            var token = await _validateTokenService.GetToken(HttpContext.Request.Headers);
            await _authService.LogoutUser(token);
            return Ok();
        }

        return StatusCode(401);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials loginCredentials)
    {
        return await _authService.LoginUser(loginCredentials);
    }
}