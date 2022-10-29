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
    public IActionResult AddReview(Guid movieId, [FromBody] ReviewModifyDto reviewModifyDto)
    {
        _reviewService.AddReview(reviewModifyDto, movieId, User.Identity.Name);
        return Ok();
    }

    [HttpPut]
    [Route("{movieId}/review/{id}/edit")]
    [Authorize]
    public IActionResult EditReview([FromBody] ReviewModifyDto reviewModifyDto, Guid movieId, Guid id)
    {
        _reviewService.EditReview(reviewModifyDto, movieId, id, User.Identity.Name);
        return Ok();
    }

    [HttpDelete]
    [Route("{movieId}/review/{id}/delete")]
    [Authorize]
    public IActionResult DeleteReview(Guid movieId, Guid id)
    {
        _reviewService.DeleteReview(movieId, id, User.Identity.Name);
        return Ok();
    }
}