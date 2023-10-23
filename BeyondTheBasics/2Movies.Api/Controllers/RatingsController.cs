using _2Movies.Api.Auth;
using _2Movies.Api.Mapping;
using _2Movies.Application.Services;
using _2Movies.Contracts.Requests.V1;
using _2Movies.Contracts.Responses.V1;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2Movies.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [ApiVersion("1.0")]
    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RateMovie([FromRoute] Guid id, [FromBody] RateMovieRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _ratingService.RateMovieAsync(id, request.Rating, userId.Value, token);
        return result ? Ok() : NotFound();
    }

    [ApiVersion("1.0")]
    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _ratingService.DeleteRatingAsync(id, userId.Value, token);

        return result ? Ok() : NotFound();
    }

    [ApiVersion("1.0")]
    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    [ProducesResponseType(typeof(IEnumerable<MovieRatingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRatings(CancellationToken token)
    {
        var user = HttpContext.GetUserId();
        var ratings = await _ratingService.GetRatingsForUserAsync(user.Value, token);

        var ratingsResponse = ratings.MapToResponse();

        return Ok(ratingsResponse);
    }
}