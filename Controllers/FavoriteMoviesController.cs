using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[ApiController]
[Route("api/favorites")]
public class FavoriteMoviesController : ControllerBase
{
    private readonly IFavoriteMoviesService _favoriteMoviesService;

    public FavoriteMoviesController(IFavoriteMoviesService favoriteMoviesService)
    {
        _favoriteMoviesService = favoriteMoviesService;
    }
    
    [HttpGet]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task<MoviesListDto> GetFavoriteMovies()
    {
        return await _favoriteMoviesService.GetFavorites(User.Identity.Name);
    }

    [HttpPost]
    [Route("{id}/add")]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task AddFavourite(Guid id)
    {
        await _favoriteMoviesService.AddFavourite(User.Identity.Name, id);
    }

    [HttpDelete]
    [Route("{id}/delete")]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task DeleteFavouriute(Guid id)
    {
        await _favoriteMoviesService.DeleteFavourite(User.Identity.Name, id);
    }
}