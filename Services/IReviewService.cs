using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IReviewService
{
    Task AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string userName);

    Task EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string userName);
}