using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IAuthService
{
    public Task<IActionResult> RegisterUser(UserRegisterDto userRegisterDto);
    public Task<IActionResult> LoginUser(LoginCredentials loginCredentials);
    public Task<IActionResult> LogoutUser(string token);
}