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
    private readonly IValidateTokenService _validateTokenService;

    public FavoriteMoviesController(IFavoriteMoviesService favoriteMoviesService, IValidateTokenService validateTokenService)
    {
        _favoriteMoviesService = favoriteMoviesService;
        _validateTokenService = validateTokenService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<MoviesListDto> GetFavoriteMovies()
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        return await _favoriteMoviesService.GetFavorites(User.Identity.Name);
    }

    [HttpPost]
    [Route("{id}/add")]
    [Authorize]
    public async Task AddFavourite(Guid id)
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        await _favoriteMoviesService.AddFavourite(User.Identity.Name, id);
    }

    [HttpDelete]
    [Route("{id}/delete")]
    [Authorize]
    public async Task DeleteFavouriute(Guid id)
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        await _favoriteMoviesService.DeleteFavourite(User.Identity.Name, id);
    }
}