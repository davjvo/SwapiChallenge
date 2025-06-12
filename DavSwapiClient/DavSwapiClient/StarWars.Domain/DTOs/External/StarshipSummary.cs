using System.Text.Json.Serialization;

namespace StarWars.Domain.DTOs.External;

public class StarshipSummary
{
    [JsonPropertyName("uid")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; set; }
}
