using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[ApiController]
[Route("api/favorites")]
public class FavoriteMoviesController : ControllerBase
{
    private IFavoriteMoviesService _favoriteMoviesService;

    public FavoriteMoviesController(IFavoriteMoviesService favoriteMoviesService)
    {
        _favoriteMoviesService = favoriteMoviesService;
    }

    [HttpGet]
    [Authorize]
    public MoviesListDto GetFavoriteMovies()
    {
        //TODO добавить валидацию токена
        return _favoriteMoviesService.GetFavorites(User.Identity.Name);
    }

    [HttpPost]
    [Route("{id}/add")]
    [Authorize]
    public void AddFavourite(Guid id)
    {
        //TODO добавить валидацию токена
        _favoriteMoviesService.AddFavourite(User.Identity.Name, id);
    }

    [HttpDelete]
    [Route("{id}/delete")]
    [Authorize]
    public void DeleteFavouriute(Guid id)
    {
        //TODO доабвить валидацию токена
        _favoriteMoviesService.DeleteFavourite(User.Identity.Name, id);
    }
}