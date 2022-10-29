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
        //посмотреть существует ли фильм в бд

        var movieEntity = await _context.Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        
        var userEntity = await _context.Users
            .Where(x => x.UserName == userName)
            .FirstOrDefaultAsync();


        if (movieEntity != null && userEntity != null)
        {
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
        } else if (userEntity == null)
        {
            throw new UserNotFoundException("Пользователь не найден");
        } else if (movieEntity == null)
        {
            throw new MovieNotFoundException("Фильм не найден");
        }
    }
}