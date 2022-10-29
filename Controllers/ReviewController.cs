using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogApi.Models.Dtos;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Controllers;

[Route("api/movie")]
[ApiController]
public class ReviewController : ControllerBase
{
    private IReviewService _reviewService;
    private IValidateTokenService _validateTokenService;
    
    public ReviewController(IReviewService reviewService, IValidateTokenService validateTokenService)
    {
        _reviewService = reviewService;
        _validateTokenService = validateTokenService;
    }

    [HttpPost]
    [Route("{movieId}/review/add")]
    [Authorize]
    public async Task<IActionResult> AddReview(Guid movieId, [FromBody] ReviewModifyDto reviewModifyDto)
    {
        await _reviewService.AddReview(reviewModifyDto, movieId, User.Identity.Name);
        return Ok();
    }

    [HttpPut]
    [Route("{movieId}/review/{id}/edit")]
    [Authorize]
    public async Task<IActionResult> EditReview([FromBody] ReviewModifyDto reviewModifyDto, Guid movieId, Guid id)
    {
        await _reviewService.EditReview(reviewModifyDto, movieId, id, User.Identity.Name);
        return Ok();
    }
}