using _2Movies.Api.Auth;
using _2Movies.Api.Mapping;
using _2Movies.Application.Services;
using _2Movies.Contracts.Requests.V1;
using _2Movies.Contracts.Responses.V1;

namespace _2Movies.Api.EndPoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetMovies";
    
    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.GetAll, async (
                [AsParameters] GetAllMoviesRequest request, IMovieService movieService,
                HttpContext context, CancellationToken token) =>
            {
                var userId = context.GetUserId();
                var options = request.MapToOptions()
                    .WithUser(userId);
                var movies = await movieService.GetAllAsync(options, token);
                var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                var moviesResponse = movies.MapToResponse(
                    request.Page.GetValueOrDefault(PagedRequest.DefaultPage), 
                    request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize), 
                    movieCount);
                return TypedResults.Ok(moviesResponse);
            })
            .WithName($"{Name}V1")
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);
        
        app.MapGet(ApiEndpoints.Movies.GetAll, async (
                [AsParameters] GetAllMoviesRequest request, IMovieService movieService,
                HttpContext context, CancellationToken token) =>
            {
                var userId = context.GetUserId();
                var options = request.MapToOptions()
                    .WithUser(userId);
                var movies = await movieService.GetAllAsync(options, token);
                var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                var moviesResponse = movies.MapToResponse(
                    request.Page.GetValueOrDefault(PagedRequest.DefaultPage), 
                    request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize), 
                    movieCount);
                return TypedResults.Ok(moviesResponse);
            })
            .WithName($"{Name}V2")
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0)
            .CacheOutput("MovieCache");
        return app;
    }
}