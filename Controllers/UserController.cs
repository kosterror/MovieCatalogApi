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

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("profile")]
    [Authorize]                     
    [Authorize(Policy = "ValidateToken")]
    public async Task<ProfileDto> GetProfile()
    {
        return await _userService.GetProfile(User.Identity.Name);
    }

    [HttpPut]
    [Route("profile")]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task UpdateProfile([FromBody] ProfileDto profileDto)
    {
        await _userService.UpdateProfile(profileDto, User.Identity.Name);
    }
}