using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
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
        var userEntity = _context.Users
            .Include(x => x.FavoriteMovies)
            .FirstOrDefault(x => x.UserName == userName);
        
        var movieEntity = _context.Movies.FirstOrDefault(x => x.Id == movieId);
        
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User with this token was not found");
        }

        if (movieEntity == null)
        {
            throw new NotFoundException("Film with this ID was not found");
        }

        if (userEntity.FavoriteMovies.Exists(x => x.Id == movieId))
        {
            throw new BadRequestException("Movie have already added to favorites");
        }
        
        userEntity.FavoriteMovies.Add(movieEntity);

        _context.SaveChanges();
    }

    public void DeleteFavourite(string userName, Guid movieId)
    {
        throw new NotImplementedException();
    }
}