namespace _2Movies.Api.EndPoints.Ratings;

public static class RatingEndpointExtensions
{
        public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app){
            app.MapRateMovie();
            app.MapDeleteRating();
            app.MapGetUserRatings();
            
            return app;
        }
}