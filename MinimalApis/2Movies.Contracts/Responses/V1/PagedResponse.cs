namespace _2Movies.Contracts.Responses.V1;

public class PagedResponse<TResponse>
{
    public required IEnumerable<TResponse> Items { get; init; } = Enumerable.Empty<TResponse>();
    
    public required int PageSize { get; init; }
    
    public required int Page { get; init; }
    
    public required int Total { get; init; }

    public bool HasNextPage => Total > PageSize * Page;

}