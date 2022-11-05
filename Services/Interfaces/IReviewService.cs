using MovieCatalogApi.Models.Dtos;

namespace MovieCatalogApi.Services;

public interface IReviewService
{
    void AddReview(ReviewModifyDto reviewModifyDto, Guid movieId, string id);
    void EditReview(ReviewModifyDto reviewModifyDto, Guid movieId, Guid reviewId, string id);
    void DeleteReview(Guid movieId, Guid reviewId, string id);
}