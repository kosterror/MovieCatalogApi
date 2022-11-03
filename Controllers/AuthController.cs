﻿using Microsoft.AspNetCore.Authorization;
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
    public TokenDto RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return _authService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Authorize]
    [Route("logout")]
    public LoggedOutDto Logout()
    {
        _validateTokenService.ValidateToken(HttpContext.Request.Headers);   
        return _authService.LogoutUser(HttpContext);
    }

    [HttpPost]
    [Route("login")]
    public TokenDto Login([FromBody] LoginCredentials loginCredentials)
    {
        return _authService.LoginUser(loginCredentials);
    }
}