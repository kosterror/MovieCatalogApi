using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services.Implementations;

public class FavoriteMoviesService : IFavoriteMoviesService
{
    private ApplicationDbContext _context;

    public FavoriteMoviesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public MoviesListDto GetFavorites(string userName)
    {
        throw new NotImplementedException();
    }

    public void AddFavourite(string userName, Guid movieId)
    {
        throw new NotImplementedException();
    }

    public void DeleteFavourite(string userName, Guid movieId)
    {
        throw new NotImplementedException();
    }
}