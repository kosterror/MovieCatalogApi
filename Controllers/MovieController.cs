using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    [Route("{page}")]
    public MoviesPagedListDto GetPage(int page)
    {
        return _movieService.GetPage(page);
    }

    [HttpGet]
    [Route("details/{id}")]
    public MovieDetailsDto GetMovieDetails(Guid id)
    {
        return _movieService.GetMovieDetails(id);
    }
}