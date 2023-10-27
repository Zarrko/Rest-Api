namespace _2Movies.Contracts.Requests.V1;

public class GetAllMoviesRequest : PagedRequest
{
    public string? Title { get; init; }
    
    public int? Year { get; init; }
    
    public string? SortBy { get; init; }
}