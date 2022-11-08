using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IMovieService
{
    Task<MoviesPagedListDto> GetPage(int page);
    Task<MovieDetailsDto> GetMovieDetails(Guid id);
}