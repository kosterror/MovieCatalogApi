using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public MovieService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<MoviesPagedListDto> GetPage(int page)
    {
        if (page < 1)
        {
            throw new BadRequestException("Wrong page");
        }

        var maxMovieCount = _configuration.GetValue<double>("PageSize");
        var filmCountInDb = _context.Movies.Count();
        var skeepCount = (int)((page - 1) * maxMovieCount);
        var takeCount = (int)Math.Min(filmCountInDb - (page - 1) * maxMovieCount, maxMovieCount);
        var pageCount = (int)Math.Ceiling(filmCountInDb / maxMovieCount);
        pageCount = pageCount == 0 ? 1 : pageCount;

        if (page > pageCount)
        {
            throw new BadRequestException("Wrong page");
        }
        
        var movieEntities = await _context
            .Movies
            .Include(movie => movie.Genres)
            .Include(movie => movie.LikedUsers)
            .OrderByDescending(movie => movie.Id)
            .Skip(skeepCount)
            .Take(takeCount)
            .ToListAsync();
        
        
        var pageInfo = new PageInfoDto
        {
            currentPage = page,
            pageCount = pageCount,
            pageSize = movieEntities.Count
        };

        var movieElementDtos = new List<MovieElementDto>();

        foreach (var movoeEntity in movieEntities)
        {
            movieElementDtos.Add(await GetMovieElementDto(movoeEntity));
        }

        var result = new MoviesPagedListDto
        {
            movies = movieElementDtos,
            pageInfo = pageInfo
        };

        return result;
    }

    public async Task<MovieDetailsDto> GetMovieDetails(Guid id)
    {
        var movieEntity = await _context
            .Movies
            .Include(movie => movie.Genres)
            .Include(movie => movie.LikedUsers)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie with this ID does not exists");
        }

        var movieDetailsDto = new MovieDetailsDto
        {
            id = movieEntity.Id,
            name = movieEntity.Name,
            poster = movieEntity.Poster,
            year = movieEntity.Year,
            country = movieEntity.Country,
            time = movieEntity.Time,
            tagline = movieEntity.Tagline,
            description = movieEntity.Description,
            director = movieEntity.Director,
            budget = movieEntity.Budget,
            fees = movieEntity.Fees,
            ageLimit = movieEntity.AgeLimit,
            genres = GetGenreDtos(movieEntity),
            reviews = await GetReviewDtos(movieEntity)
        };

        return movieDetailsDto;
    }

    private async Task<MovieElementDto> GetMovieElementDto(MovieEntity movieEntity)
    {
        var movieElementDto = new MovieElementDto
        {
            id = movieEntity.Id,
            name = movieEntity.Name,
            poster = movieEntity.Poster,
            year = movieEntity.Year,
            country = movieEntity.Country,
            genres = GetGenreDtos(movieEntity),
            reviews = await GetReviewShortDtos(movieEntity)
        };

        return movieElementDto;
    }

    private static List<GenreDto> GetGenreDtos(MovieEntity movieEntity)
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

    private async Task<List<ReviewShortDto>> GetReviewShortDtos(MovieEntity movieEntity)
    {
        var reviewEntities = await _context
            .Reviews
            .Where(x => x.Movie.Id == movieEntity.Id)
            .ToListAsync();


        var reviewShortDtos = reviewEntities
            .Select(reviewEntity => new ReviewShortDto
            {
                id = reviewEntity.Id,
                rating = reviewEntity.Rating
            })
            .ToList();

        return reviewShortDtos;
    }

    private async Task<List<ReviewDto>> GetReviewDtos(MovieEntity movieEntity)
    {
        var reviewEntities = await _context
            .Reviews
            .Where(x => x.Movie.Id == movieEntity.Id)
            .Include(x => x.User)
            .ToListAsync();

        var reviewDtos =
            reviewEntities
                .Select(entity => new ReviewDto
                {
                    id = entity.Id,
                    rating = entity.Rating,
                    reviewText = entity.ReviewText,
                    isAnonymous = entity.IsAnonymous,
                    createDateTime = entity.CreatedDateTime,
                    author = GetUserShortDto(entity)
                })
                .ToList();

        return reviewDtos;
    }

    private static UserShortDto GetUserShortDto(ReviewEntity reviewEntity)
    {
        var userShortDto = new UserShortDto
        {
            userId = reviewEntity.User.Id,
            nickName = reviewEntity.User.UserName,
            avatar = reviewEntity.User.Avatar
        };

        return userShortDto;
    }
}