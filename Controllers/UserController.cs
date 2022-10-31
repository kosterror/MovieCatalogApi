using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/account")]
[ApiController]
public class UserController : Controller
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("profile")]
    [Authorize]
    public ProfileDto GetProfile()
    {
        //TODO проверить валидность токена
        return _userService.GetProfile(User.Identity.Name);
    }

    [HttpPut]
    [Route("profile")]
    [Authorize]
    public void UpdateProfile([FromBody] ProfileDto profileDto)
    {
        //TODO проверить валидность токена
        _userService.UpdateProfile(profileDto, User.Identity.Name);
    }
}