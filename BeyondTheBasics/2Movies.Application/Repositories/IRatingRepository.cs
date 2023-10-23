using System.Collections;
using _2Movies.Application.Models;

namespace _2Movies.Application.Repositories;

public interface IRatingRepository
{
    Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token);
    
    Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default);
    
    Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken token = default);

    Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default);

    Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token);

}