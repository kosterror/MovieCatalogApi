using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IReviewService
{
    void AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string userName);
    void EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string userName);
    void DeleteReview(Guid movieId, Guid reviewId, string userName);
}