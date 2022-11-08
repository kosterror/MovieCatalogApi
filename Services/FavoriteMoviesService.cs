using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services;

public class FavoriteMoviesService : IFavoriteMoviesService
{
    private readonly ApplicationDbContext _context;

    public FavoriteMoviesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MoviesListDto> GetFavorites(string id)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Id.ToString() == id)
            .FirstOrDefaultAsync();


        /*
        * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его Id из валидного токена
        * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User with this token was not found");
        }

        var movies = await _context
            .Movies
            .Include(x => x.Genres)
            .Include(x => x.LikedUsers)
            .Where(x => x.LikedUsers.Contains(userEntity))
            .ToListAsync();

        var moviesListDto = new MoviesListDto();

        foreach (var movie in movies)
        {
            moviesListDto.movies.Add(await GetMovieElementDto(movie));
        }

        return moviesListDto;
    }

    public async Task AddFavourite(string id, Guid movieId)
    {
        var userEntity = await _context
            .Users
            .Include(x => x.FavoriteMovies)
            .Where(x => x.Id.ToString() == id)
            .FirstOrDefaultAsync();

        var movieEntity = await _context
            .Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        /*
        * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его Id из валидного токена
        * считаю, что это ошибка 401
        */
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

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFavourite(string id, Guid movieId)
    {
        var userEntity = await _context
            .Users
            .Include(x => x.FavoriteMovies)
            .Where(x => x.Id.ToString() == id)
            .FirstOrDefaultAsync();

        var movieEntity = await _context
            .Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        /*
        * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его Id из валидного токена
        * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User with this token was not found");
        }

        if (movieEntity == null)
        {
            throw new NotFoundException("Film with this ID was not found");
        }

        if (!userEntity.FavoriteMovies.Exists(x => x.Id == movieId))
        {
            throw new BadRequestException("Not-existing user favorite movie");
        }

        //TODO исправить удаление не по индексу, а сущности, НО АСИНХРОННО
        var index = userEntity
            .FavoriteMovies
            .FindIndex(x => x.Id == movieId);
        userEntity.FavoriteMovies.RemoveAt(index);
        await _context.SaveChangesAsync();
    }

    // private async Task<MoviesListDto> GetMovieLsitDto(List<MovieEntity> entities)
    // {
    //     var result = new MoviesListDto();
    //
    //     foreach (var entity in entities)
    //     {
    //         result.movies.Add(await GetMovieElementDto(entity));
    //     }
    //
    //     return result;
    // }

    private async Task<MovieElementDto> GetMovieElementDto(MovieEntity movieEntity)
    {
        var result = new MovieElementDto
        {
            id = movieEntity.Id,
            name = movieEntity.Name,
            poster = movieEntity.Poster,
            year = movieEntity.Year,
            country = movieEntity.Country,
            genres = GetGenres(movieEntity),
            reviews = await GetShotReviews(movieEntity)
        };

        return result;
    }

    private static List<GenreDto> GetGenres(MovieEntity movieEntity)
    {
        var listGenreDtos = new List<GenreDto>();

        foreach (var genreEntity in movieEntity.Genres)
        {
            var genreDto = new GenreDto
            {
                id = genreEntity.Id,
                name = genreEntity.Name
            };

            listGenreDtos.Add(genreDto);
        }

        return listGenreDtos;
    }

    private async Task<List<ReviewShortDto>> GetShotReviews(MovieEntity movieEntity)
    {
        var reviewShortDtos = new List<ReviewShortDto>();

        var reviews = await _context.Reviews
            .Include(x => x.User) //будто бы лишняя строка           
            .Include(x => x.Movie) //и эта тоже
            .Where(x => x.Movie.Id == movieEntity.Id)
            .ToListAsync();

        foreach (var review in reviews)
        {
            var shortReviewDto = new ReviewShortDto
            {
                id = review.Id,
                rating = review.Rating
            };

            reviewShortDtos.Add(shortReviewDto);
        }

        return reviewShortDtos;
    }
}