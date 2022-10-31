using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IFavoriteMoviesService
{
    MoviesListDto GetFavorites(string userName);
    void AddFavourite(string userName, Guid movieId);
    void DeleteFavourite(string userName, Guid movieId);
}