﻿using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    /*
     * ВАЖНО! кажется будто бы доставать фильм и пользователя везде необязательно,
     * НО это является дополнительной проверкой на валидность входных данных
     */

    public void AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string id)
    {
        var userEntity = _context
            .Users
            .FirstOrDefault(x => x.Id.ToString() == id);

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User by with token not found");
        }

        //TODO красиво собирать ошибки
        var movieEntity = _context
            .Movies
            .FirstOrDefault(x => x.Id == movieId);

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie by this ID not found");
        }

        var oldReview = _context
            .Reviews
            .FirstOrDefault(x => x.User.Id == userEntity.Id
                                 && x.Movie.Id == movieEntity.Id);

        if (oldReview != null)
        {
            throw new ReviewAlreadyExistsException("User already has review for this movie");
        }

        var reviewEntity = new ReviewEntity
        {
            Id = Guid.NewGuid(),
            User = userEntity,
            Movie = movieEntity,
            ReviewText = reviewModifyDto.reviewText,
            Rating = reviewModifyDto.rating,
            IsAnonymous = reviewModifyDto.isAnonymous,
            CreatedDateTime = DateTime.Now.ToUniversalTime()
        };

        _context.Reviews.Add(reviewEntity);
        _context.SaveChanges();
    }

    public void EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string id)
    {
        var userEntity = _context
            .Users
            .FirstOrDefault(x => x.Id.ToString() == id);

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User by with token not found");
        }

        //TODO красиво собирать ошибки
        var movieEntity = _context
            .Movies
            .FirstOrDefault(x => x.Id == movieId);

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = _context
            .Reviews
            .FirstOrDefault(x => x.Id == reviewId
                                 && x.Movie.Id == movieEntity.Id);

        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }

        if (reviewEntity.User.Id.ToString() != userEntity.Id.ToString())
        {
            throw new NotEnoughtRightsException("you can not edit someone else's review");
        }
        
        reviewEntity.ReviewText = reviewModifyDto.reviewText;
        reviewEntity.Rating = reviewModifyDto.rating;
        reviewEntity.IsAnonymous = reviewModifyDto.isAnonymous;

        _context.SaveChanges();
    }

    public void DeleteReview(Guid movieId, Guid reviewId, string id)
    {
        var userEntity = _context
            .Users
            .FirstOrDefault(x => x.Id.ToString() == id);

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new PermissionDeniedException("User by with token not found");
        }


        //TODO красиво собирать ошибки
        var movieEntity = _context
            .Movies
            .FirstOrDefault(x => x.Id == movieId);

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = _context
            .Reviews
            .FirstOrDefault(x => x.Id == reviewId
                                 && x.Movie.Id == movieEntity.Id);
        
        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }
        
        
        if (reviewEntity.User.Id.ToString() != userEntity.Id.ToString())
        {
            throw new NotEnoughtRightsException("you can not delete someone else's review");
        }
        
        _context.Reviews.Remove(reviewEntity);
        _context.SaveChanges();
    }
}