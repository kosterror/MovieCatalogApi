using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IAuthService
{
    public JsonResult RegisterUser(UserRegisterDto userRegisterDto);
    public JsonResult LoginUser(LoginCredentials loginCredentials);
    public JsonResult LogoutUser(string token);
}