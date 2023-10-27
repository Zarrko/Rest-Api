using _2Movies.Api.EndPoints.Movies;
using _2Movies.Api.EndPoints.Ratings;

namespace _2Movies.Api.EndPoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMovieEndpoints();
        app.MapRatingEndpoints();
        return app;
    }
}