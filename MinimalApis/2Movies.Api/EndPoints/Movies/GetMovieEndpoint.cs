using _2Movies.Api.Auth;
using _2Movies.Api.Mapping;
using _2Movies.Application.Services;
using _2Movies.Contracts.Responses.V1;
using Microsoft.AspNetCore.Http.HttpResults;

namespace _2Movies.Api.EndPoints.Movies;

public static class GetMovieEndpoint
{
    public const string Name = "GetMovie";
    
    public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.Get, async (
                string idOrSlug, IMovieService movieService,
                HttpContext context, CancellationToken token) =>
            {
                var userId = context.GetUserId();

                var movie = Guid.TryParse(idOrSlug, out var id)
                    ? await movieService.GetByIdAsync(id, userId, token)
                    : await movieService.GetBySlugAsync(idOrSlug, userId, token);
                if (movie is null)
                {
                    return Results.NotFound();
                }

                var response = movie.MapToResponse();
                return TypedResults.Ok(response);
            })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput("MovieCache");
        return app;
    }
}