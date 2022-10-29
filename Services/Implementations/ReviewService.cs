using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services.Implementations;

public class ReviewService : IReviewService
{
    private ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string userName)
    {
        var userEntity = await _context.Users
            .Where(x => x.UserName == userName)
            .FirstOrDefaultAsync();
     
        if (userEntity == null)
        {
            throw new NotFoundException("User not found");
        }
        
        var movieEntity = await _context.Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }
        
        var oldReview = await _context.Reviews
            .Where(x => x.User.Id == userEntity.Id)
            .FirstOrDefaultAsync();

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

        await _context.Reviews.AddAsync(reviewEntity);
        await _context.SaveChangesAsync();
    }

    public async Task EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string userName)
    {
        //вообще пользователя и фильм вытаскивать нет смысла, но нам могут подсунуть невалидные данные,
        //а за это нужнj ругать
        
        var userEntity = await _context.Users
            .Where(x => x.UserName == userName)
            .FirstOrDefaultAsync();
     
        if (userEntity == null)
        {
            throw new NotFoundException("User not found");
        }
        
        var movieEntity = await _context.Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();
        
        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = await _context.Reviews
            .Where(x => x.Id == reviewId)
            .FirstOrDefaultAsync();

        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }

        reviewEntity.ReviewText = reviewModifyDto.reviewText;
        reviewEntity.Rating = reviewModifyDto.rating;
        reviewEntity.IsAnonymous = reviewModifyDto.isAnonymous;

        await _context.SaveChangesAsync();
    }
    
}