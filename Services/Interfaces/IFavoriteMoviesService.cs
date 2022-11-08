using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IFavoriteMoviesService
{
    Task<MoviesListDto> GetFavorites(string id);
    Task AddFavourite(string id, Guid movieId);
    Task DeleteFavourite(string id, Guid movieId);
}