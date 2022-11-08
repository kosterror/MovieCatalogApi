using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IReviewService
{
    Task AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string id);
    Task EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string id);
    Task DeleteReview(Guid movieId, Guid reviewId, string id);
}