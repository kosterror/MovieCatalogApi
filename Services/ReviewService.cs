using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
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

    public async Task AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string id)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Id.ToString() == id)
            .FirstOrDefaultAsync();

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new UserNotFoundException("User by with token not found");
        }
        
        var movieEntity = await _context
            .Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie by this ID not found");
        }

        var oldReview = await _context
            .Reviews
            .Where(x => x.User.Id == userEntity.Id
                        && x.Movie.Id == movieEntity.Id)
            .FirstOrDefaultAsync();


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

        await _context.Reviews.AddAsync(reviewEntity);
        await _context.SaveChangesAsync();
    }

    public async Task EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string id)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Id.ToString() == id)
            .FirstOrDefaultAsync();

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new UserNotFoundException("User by with token not found");
        }

        var movieEntity = await _context
            .Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = await _context
            .Reviews
            .Where(x => x.Id == reviewId
                        && x.Movie.Id == movieEntity.Id)
            .FirstOrDefaultAsync();

        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }

        if (reviewEntity.User.Id.ToString() != userEntity.Id.ToString())
        {
            throw new NotEnoughtRightsException("You can not edit someone else's review");
        }

        reviewEntity.ReviewText = reviewModifyDto.reviewText;
        reviewEntity.Rating = reviewModifyDto.rating;
        reviewEntity.IsAnonymous = reviewModifyDto.isAnonymous;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteReview(Guid movieId, Guid reviewId, string id)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Id.ToString() == id)
            .FirstOrDefaultAsync();

        /*
         * вероятность сюда попасть - почти нулевая, т.к. мы не нашли пользователя по его ID из валидного токена
         * считаю, что это ошибка 401
        */
        if (userEntity == null)
        {
            throw new UserNotFoundException("User by with token not found");
        }
        
        
        var movieEntity = await _context
            .Movies
            .Where(x => x.Id == movieId)
            .FirstOrDefaultAsync();

        if (movieEntity == null)
        {
            throw new NotFoundException("Movie not found");
        }

        var reviewEntity = await _context
            .Reviews
            .Include(review => review.User)
            .Where(x => x.Id == reviewId
                        && x.Movie.Id == movieEntity.Id)
            .FirstOrDefaultAsync();

        if (reviewEntity == null)
        {
            throw new NotFoundException("Review not found");
        }


        if (reviewEntity.User.Id != userEntity.Id)
        {
            throw new NotEnoughtRightsException("You can not delete someone else's review");
        }

        _context.Reviews.Remove(reviewEntity);
        await _context.SaveChangesAsync();
    }
}