﻿using Microsoft.EntityFrameworkCore;
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
        var movieEntities = _context.Movies
            .Include(movie => movie.Genres)
            .Include(movie => movie.LikedUsers)
            .ToList();
        
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

    public MovieDetailsDto GetMovieDetails(Guid id)
    {
        var movieEntity = _context.Movies
            .Include(movie => movie.Genres)
            .Include(movie => movie.LikedUsers)
            .FirstOrDefault(x => x.Id == id);
        
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
            reviews = GetReviewDtos(movieEntity)
        };

        return movieDetailsDto;
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

    private List<ReviewDto> GetReviewDtos(MovieEntity movieEntity)
    {
        var reviewEntities = _context.Reviews
            .Where(x => x.Movie.Id == movieEntity.Id)
            .Include(x => x.User)
            .ToList();

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

    private UserShortDto GetUserShortDto(ReviewEntity reviewEntity)
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