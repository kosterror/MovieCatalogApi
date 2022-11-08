using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/account")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IValidateTokenService _validateTokenService;

    public UserController(IUserService userService, IValidateTokenService validateTokenService)
    {
        _userService = userService;
        _validateTokenService = validateTokenService;
    }

    [HttpGet]
    [Route("profile")]
    [Authorize]                     
    public async Task<ProfileDto> GetProfile()
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        return await _userService.GetProfile(User.Identity.Name);
    }

    [HttpPut]
    [Route("profile")]
    [Authorize]
    public async Task UpdateProfile([FromBody] ProfileDto profileDto)
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        await _userService.UpdateProfile(profileDto, User.Identity.Name);
    }
}