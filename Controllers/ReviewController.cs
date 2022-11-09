using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/movie")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    [Route("{movieId}/review/add")]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task AddReview(Guid movieId, [FromBody] ReviewModifyDto reviewModifyDto)
    {
        await _reviewService.AddReview(reviewModifyDto, movieId, User.Identity.Name);
    }

    [HttpPut]
    [Route("{movieId}/review/{id}/edit")]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task EditReview([FromBody] ReviewModifyDto reviewModifyDto, Guid movieId, Guid id)
    {
        await _reviewService.EditReview(reviewModifyDto, movieId, id, User.Identity.Name);
    }

    [HttpDelete]
    [Route("{movieId}/review/{id}/delete")]
    [Authorize]
    [Authorize(Policy = "ValidateToken")]
    public async Task DeleteReview(Guid movieId, Guid id)
    {
        await _reviewService.DeleteReview(movieId, id, User.Identity.Name);
    }
}