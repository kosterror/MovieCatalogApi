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
    private IValidateTokenService _validateTokenService;

    public FavoriteMoviesController(IFavoriteMoviesService favoriteMoviesService, IValidateTokenService validateTokenService)
    {
        _favoriteMoviesService = favoriteMoviesService;
        _validateTokenService = validateTokenService;
    }
    
    [HttpGet]
    [Authorize]
    public MoviesListDto GetFavoriteMovies()
    {
        _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        return _favoriteMoviesService.GetFavorites(User.Identity.Name);
    }

    [HttpPost]
    [Route("{id}/add")]
    [Authorize]
    public void AddFavourite(Guid id)
    {
        _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        _favoriteMoviesService.AddFavourite(User.Identity.Name, id);
    }

    [HttpDelete]
    [Route("{id}/delete")]
    [Authorize]
    public void DeleteFavouriute(Guid id)
    {
        _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        _favoriteMoviesService.DeleteFavourite(User.Identity.Name, id);
    }
}