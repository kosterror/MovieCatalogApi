﻿using Microsoft.EntityFrameworkCore;
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

    public MoviesListDto GetFavorites(string id)
    {
        var userEntity = _context
            .Users
            .FirstOrDefault(x => x.Id.ToString() == id);


        /*
        * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его Id из валидного токена
        * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User with this token was not found");
        }

        var movies = _context
            .Movies
            .Include(x => x.Genres)
            .Include(x => x.LikedUsers)
            .Where(x => x.LikedUsers.Contains(userEntity))
            .ToList();

        var moviesListDto = new MoviesListDto();

        foreach (var movie in movies)
        {
            moviesListDto.movies.Add(GetMovieElementDto(movie));
        }

        return moviesListDto;
    }

    public void AddFavourite(string id, Guid movieId)
    {
        var userEntity = _context
            .Users
            .Include(x => x.FavoriteMovies)
            .FirstOrDefault(x => x.Id.ToString() == id);

        var movieEntity = _context.Movies.FirstOrDefault(x => x.Id == movieId);

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

        _context.SaveChanges();
    }

    public void DeleteFavourite(string id, Guid movieId)
    {
        var userEntity = _context
            .Users
            .Include(x => x.FavoriteMovies)
            .FirstOrDefault(x => x.Id.ToString() == id);

        var movieEntity = _context.Movies.FirstOrDefault(x => x.Id == movieId);
        
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

        var index = userEntity.FavoriteMovies.FindIndex(x => x.Id == movieId);
        userEntity.FavoriteMovies.RemoveAt(index);
        _context.SaveChanges();
    }

    private MoviesListDto GetMovieLsitDto(List<MovieEntity> entities)
    {
        var result = new MoviesListDto();

        foreach (var entity in entities)
        {
            result.movies.Add(GetMovieElementDto(entity));
        }

        return result;
    }

    private MovieElementDto GetMovieElementDto(MovieEntity movieEntity)
    {
        var result = new MovieElementDto
        {
            id = movieEntity.Id,
            name = movieEntity.Name,
            poster = movieEntity.Poster,
            year = movieEntity.Year,
            country = movieEntity.Country,
            genres = GetGenres(movieEntity),
            reviews = GetShotReviews(movieEntity)
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

    private List<ReviewShortDto> GetShotReviews(MovieEntity movieEntity)
    {
        var reviewShortDtos = new List<ReviewShortDto>();

        var reviews = _context.Reviews
            .Include(x => x.User)                   //будто бы лишняя строка           
            .Include(x => x.Movie)                  //и эта тоже
            .Where(x => x.Movie.Id == movieEntity.Id).ToList();

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