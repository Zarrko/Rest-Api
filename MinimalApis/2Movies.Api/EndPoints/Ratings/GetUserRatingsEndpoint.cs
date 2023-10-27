using _2Movies.Api.Auth;
using _2Movies.Api.Mapping;
using _2Movies.Application.Services;
using _2Movies.Contracts.Responses.V1;

namespace _2Movies.Api.EndPoints.Ratings;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";
    
    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Ratings.GetUserRatings,
                async (HttpContext context, IRatingService ratingService,
                    CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var ratings = await ratingService.GetRatingsForUserAsync(userId!.Value, token);
                    return TypedResults.Ok(ratings);
                })
            .WithName(Name)
            .Produces<MovieRatingResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
        
        return app;
    }
}