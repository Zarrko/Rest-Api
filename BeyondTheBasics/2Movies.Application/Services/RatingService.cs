using _2Movies.Application.Models;
using _2Movies.Application.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace _2Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;

    public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }
    
    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token)
    {
        if (rating > 5 || rating < 0)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = "Rating",
                    ErrorMessage = "Rating must be between 1 and 5"
                }
            });
        }

        var movieExists = await _movieRepository.ExistsByIdAsync(movieId, token);
        if (!movieExists)
        {
            return false;
        }

        return await _ratingRepository.RateMovieAsync(movieId, rating, userId, token);
    }

    public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.DeleteRatingAsync(movieId, userId, token);
    }

    public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        return await _ratingRepository.GetRatingsForUserAsync(userId, token);
    }
}