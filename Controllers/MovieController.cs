using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    [Route("{page}")]
    public async Task<MoviesPagedListDto> GetPage(int page)
    {
        return await _movieService.GetPage(page);
    }

    [HttpGet]
    [Route("details/{id}")]
    public async Task<MovieDetailsDto> GetMovieDetails(Guid id)
    {
        return await _movieService.GetMovieDetails(id);
    }
}