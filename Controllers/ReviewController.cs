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
    private readonly IValidateTokenService _validateTokenService;
    
    public ReviewController(IReviewService reviewService, IValidateTokenService validateTokenService)
    {
        _reviewService = reviewService;
        _validateTokenService = validateTokenService;
    }

    [HttpPost]
    [Route("{movieId}/review/add")]
    [Authorize]
    public async Task AddReview(Guid movieId, [FromBody] ReviewModifyDto reviewModifyDto)
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        await _reviewService.AddReview(reviewModifyDto, movieId, User.Identity.Name);
    }

    [HttpPut]
    [Route("{movieId}/review/{id}/edit")]
    [Authorize]
    public async Task EditReview([FromBody] ReviewModifyDto reviewModifyDto, Guid movieId, Guid id)
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        await _reviewService.EditReview(reviewModifyDto, movieId, id, User.Identity.Name);
    }

    [HttpDelete]
    [Route("{movieId}/review/{id}/delete")]
    [Authorize]
    public async Task DeleteReview(Guid movieId, Guid id)
    {
        await _validateTokenService.ValidateToken(HttpContext.Request.Headers);
        await _reviewService.DeleteReview(movieId, id, User.Identity.Name);
    }
}