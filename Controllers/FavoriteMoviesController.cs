using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

public class FavoriteMoviesController
{
    private IFavoriteMoviesService _favoriteMoviesService;

    public FavoriteMoviesController(IFavoriteMoviesService favoriteMoviesService)
    {
        _favoriteMoviesService = favoriteMoviesService;
    }
}