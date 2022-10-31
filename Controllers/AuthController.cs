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
    public JsonResult RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return _authService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Authorize]
    [Route("logout")]
    public JsonResult Logout()
    {
        //нельзя давать возможность разлогиниваться под одним токеном несколько раз
        _validateTokenService.ValidateToken(HttpContext.Request.Headers);

        return _authService.LogoutUser(HttpContext.Request.Headers);
    }

    [HttpPost]
    [Route("login")]
    public JsonResult Login([FromBody] LoginCredentials loginCredentials)
    {
        return _authService.LoginUser(loginCredentials);
    }
}