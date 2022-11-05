using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IFavoriteMoviesService
{
    MoviesListDto GetFavorites(string id);
    void AddFavourite(string id, Guid movieId);
    void DeleteFavourite(string id, Guid movieId);
}