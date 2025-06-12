using System.Text.Json.Serialization;

namespace StarWars.Domain.DTOs.External;

public class StarshipResponse
{
    public class StarshipResult
    {
        [JsonPropertyName("properties")]
        public StarshipSummary? Starship { get; set; }
    }

    [JsonPropertyName("result")]
    public StarshipResult? Result { get; set; }
}
