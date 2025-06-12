using System.Text.Json.Serialization;

namespace StarWars.Domain.DTOs.External;

public class StarshipPageResponse
{
    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("results")]
    public IEnumerable<StarshipSummary>? Starships { get; set; }
}
