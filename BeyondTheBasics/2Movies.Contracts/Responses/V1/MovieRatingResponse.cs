namespace _2Movies.Contracts.Responses.V1;

public class MovieRatingResponse
{
    public required Guid MovieId { get; init; }
    
    public required string Slug { get; init; }
    
    public required int Rating { get; init; }
}