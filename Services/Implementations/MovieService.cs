using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services.Implementations;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public MovieService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public MoviesPagedListDto GetPage(int page)
    {
        var movieEntities = _context.Movies.ToList();
        
        var pageInfo = new PageInfoDto
        {
            currentPage = page,
            pageCount = (int)Math.Ceiling(movieEntities.Count / (double)page) == 0 ? 1 : (int)Math.Ceiling(movieEntities.Count / (double)page),
            pageSize = _configuration.GetValue<int>("PageSize")
        };

        if (page > pageInfo.pageCount)
        {
            throw new BadRequestException("Wrong page");
        }
        
        var movieElementDtos = new List<MovieElementDto>();

        //получать с (page - 1) * pageSize по page * pageSize - 1
        for (var i = (page - 1) * pageInfo.pageSize; i < page * pageInfo.pageSize - 1 && i < movieEntities.Count; i++)
        {
            movieElementDtos.Add(GetMovieElementDto(movieEntities[i]));
        }

        
        var result = new MoviesPagedListDto
        {
            movies = movieElementDtos,
            pageInfo = pageInfo
        };

        return result;
    }

    private MovieElementDto GetMovieElementDto(MovieEntity movieEntity)
    {
        var movieElementDto = new MovieElementDto
        {
            id = movieEntity.Id,
            name = movieEntity.Name,
            poster = movieEntity.Poster,
            year = movieEntity.Year,
            country = movieEntity.Country,
            genres = GetGenreDtos(movieEntity),
            reviews = GetReviewShortDtos(movieEntity)
        };

        return movieElementDto;
    }

    private List<GenreDto> GetGenreDtos(MovieEntity movieEntity)
    {
        var genreEntities = movieEntity.Genres;

        var genreDtos =
            genreEntities
                .Select(genreEntity => new GenreDto
                {
                    id = genreEntity.Id,
                    name = genreEntity.Name
                })
                .ToList();

        return genreDtos;
    }

    private List<ReviewShortDto> GetReviewShortDtos(MovieEntity movieEntity)
    {
        var reviewEntities = _context.Reviews.Where(x => x.Movie.Id == movieEntity.Id).ToList();

        var reviewShortDtos =
            reviewEntities
                .Select(reviewEntity => new ReviewShortDto
                {
                    id = reviewEntity.Id,
                    rating = reviewEntity.Rating
                })
                .ToList();

        return reviewShortDtos;
    }
}