using System.Text.Json.Serialization;

namespace _2Movies.Contracts.Responses.V1;

public abstract class HalResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Link> Links { get; set; } = new();
}

public class Link
{
    public required string Href { get; set; }
    
    public required string Rel { get; set; }
    
    public required string Type { get; set; }
}