using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string userName)
    {
        var userEntity = _context.Users
            .FirstOrDefault(x => x.UserName == userName);

        if (userEntity == null)
        {
            throw new NotFoundException("User not found");
        }

        var movieEntity = _context.Movies
            .FirstOrDefault(x => x.Id == movieId);

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var oldReview = _context.Reviews
            .FirstOrDefault(x => x.User.Id == userEntity.Id 
                                 && x.Movie.Id == movieEntity.Id);
        
        if (oldReview != null)
        {
            throw new ReviewAlreadyExistsException("User has already had review for this movie");
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

    public void EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string userName)
    {
        //вообще пользователя и фильм вытаскивать нет смысла, но нам могут подсунуть невалидные данные,
        //а за это нужно ругать

        var userEntity = _context.Users
            .FirstOrDefault(x => x.UserName == userName);

        if (userEntity == null)
        {
            throw new NotFoundException("User not found");
        }

        var movieEntity = _context.Movies
            .FirstOrDefault(x => x.Id == movieId);

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = _context.Reviews
            .FirstOrDefault(x => x.Id == reviewId
                                 && x.User.Id == userEntity.Id
                                 && x.Movie.Id == movieEntity.Id);

        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }
        
        reviewEntity.ReviewText = reviewModifyDto.reviewText;
        reviewEntity.Rating = reviewModifyDto.rating;
        reviewEntity.IsAnonymous = reviewModifyDto.isAnonymous;

        _context.SaveChanges();
    }

    public void DeleteReview(Guid movieId, Guid reviewId, string userName)
    {
        var userEntity = _context.Users
            .FirstOrDefault(x => x.UserName == userName);

        if (userEntity == null)
        {
            throw new NotFoundException("User not found");
        }

        var movieEntity = _context.Movies
            .FirstOrDefault(x => x.Id == movieId);

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = _context.Reviews
            .FirstOrDefault(x => x.Id == reviewId 
                                 && x.User.Id == userEntity.Id
                                 && x.Movie.Id == movieEntity.Id);

        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }

        _context.Reviews.Remove(reviewEntity);
        _context.SaveChanges();
    }
}