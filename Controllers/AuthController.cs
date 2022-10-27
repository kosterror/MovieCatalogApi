using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/account")]
[ApiController]
public class AuthController : ControllerBase
{
    private IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        return await _service.RegisterUser(userRegisterDto);
    }


    // [HttpGet]
    // [Authorize]
    // [Route("test_login")]
    // public IActionResult TestAuth()
    // {
    //     return Ok($"Ваш логин: {User.Identity.Name}");
    // }

    [HttpGet]
    [Authorize]
    [Route("get-information")]
    public IActionResult GetInfromationFromToken()
    {
        return Ok($"{User.Identity?.Name}");
    }
}