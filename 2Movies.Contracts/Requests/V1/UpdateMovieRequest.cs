namespace _2Movies.Contracts.Requests.V1;

public class UpdateMovieRequest
{
    // required: Property must be initialized through Object/Constructor initialization
    public required string Title { get; init; }
    
    public required int YearOfRelease { get; init; }

    public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
}