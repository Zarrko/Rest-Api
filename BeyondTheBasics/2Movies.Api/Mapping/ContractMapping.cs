using _2Movies.Application.Models;
using _2Movies.Contracts.Requests.V1;
using _2Movies.Contracts.Responses.V1;

namespace _2Movies.Api.Mapping;

public static class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
        };
    }

    public static MovieResponse MapToResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres,
            Title = movie.Title,
            Slug = movie.Slug,
            UserRating = movie.UserRating,
            Rating = movie.Rating,
        };
    }

    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies, int page, int pageSize, int totalCount)
    {
        return new MoviesResponse
        {
            Items = movies.Select(MapToResponse),
            Page = page,
            PageSize = pageSize,
            Total = totalCount
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Id = id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
        };
    }
    
    public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
    {
        return ratings.Select(x => new MovieRatingResponse
        {
            MovieId = x.MovieId,
            Slug = x.Slug,
            Rating = x.Rating,
        });
    }

    public static GetAllMoviesOptions MapToOptions(this GetAllMoviesRequest request)
    {
        return new GetAllMoviesOptions
        {
            Title = request.Title,
            YearOfRelease = request.Year,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted : request.SortBy.StartsWith('-') ? SortOrder.Desc : SortOrder.Asc,
            Page = request.Page,
            PageSize = request.PageSize,
                
        };
    }

    public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions getAllMoviesOptions, Guid? userId)
    {
        if (userId != null) getAllMoviesOptions.UserId = userId.Value;
        return getAllMoviesOptions;
    }
}