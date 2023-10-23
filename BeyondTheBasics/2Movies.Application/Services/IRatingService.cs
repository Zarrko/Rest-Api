using _2Movies.Application.Models;

namespace _2Movies.Application.Services;

public interface IRatingService
{
    Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token);

    Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default);

    Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
}