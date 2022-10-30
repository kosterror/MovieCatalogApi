using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Exceptions;
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
    public JsonResult RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return _authService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Authorize]
    [Route("logout")]
    public async Task<JsonResult> Logout()
    {
        //нельзя давать возможность два раза ралогиниться под одним и тем же токеном
        if (!await _validateTokenService.IsValidToken(HttpContext.Request.Headers))
        {
            throw new PermissionDeniedException("Token is expired");
        }
        
        var token = await _validateTokenService.GetToken(HttpContext.Request.Headers);
        return _authService.LogoutUser(token);
    }

    [HttpPost]
    [Route("login")]
    public JsonResult Login([FromBody] LoginCredentials loginCredentials)
    {
        return _authService.LoginUser(loginCredentials);
    }
}