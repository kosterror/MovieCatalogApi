using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IMovieService
{
    MoviesPagedListDto GetPage(int page);
}