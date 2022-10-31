using MovieCatalogApi.Models;

namespace MovieCatalogApi.Services.Implementations;

public class FavoriteMoviesService : IFavoriteMoviesService
{
    private ApplicationDbContext _context;

    public FavoriteMoviesService(ApplicationDbContext context)
    {
        _context = context;
    }
}