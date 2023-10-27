namespace _2Movies.Api.EndPoints.Movies;

public static class MovieEndpointExtensions
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app){
        app.MapGetMovie();
        app.MapCreateMovie(); 
        app.MapGetAllMovies();
        app.MapUpdateMovie();
        app.MapDeleteMovie();
        
        return app;
    }
}