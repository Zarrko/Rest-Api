namespace Movies.Contracts.Requests;

public class CreateMovieRequest
{
    // required: Property must be initialized through Object/Constructor initialization
    public required string Title { get; init; }
    
    public required int YearOfRelease { get; init; }

    public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
}