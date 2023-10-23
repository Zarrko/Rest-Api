using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    
    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(x => x.Id == id);
        return Task.FromResult<Movie?>(movie);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Movie>>(_movies.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var movieIdx = _movies.FindIndex(x => x.Id == movie.Id);
        if (movieIdx == -1)
        {
            return Task.FromResult<bool>(false);
        }

        _movies[movieIdx] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removeCount = _movies.RemoveAll(x => x.Id == id);
        var hasMovieBeenRemoved = removeCount > 0;
        
        return Task.FromResult<bool>(hasMovieBeenRemoved);
    }
}