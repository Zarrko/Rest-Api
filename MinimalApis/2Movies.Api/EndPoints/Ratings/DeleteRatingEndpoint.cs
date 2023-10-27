using _2Movies.Api.Auth;
using _2Movies.Application.Services;

namespace _2Movies.Api.EndPoints.Ratings;

public static class DeleteRatingEndpoint
{
    public const string Name = "DeleteRating";
    
    public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.DeleteRating,
                async (Guid id, HttpContext context, IRatingService ratingService,
                    CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var result = await ratingService.DeleteRatingAsync(id, userId!.Value, token);
                    return result ? Results.Ok() : Results.NotFound();
                })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        
        return app;
    }
}