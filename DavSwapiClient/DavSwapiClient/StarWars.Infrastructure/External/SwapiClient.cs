using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using StarWars.Domain.Config;
using StarWars.Domain.DTOs.External;
using System.Text.Json;

namespace StarWars.Infrastructure.External;

public interface ISwapiClient
{
    Task<StarshipPageResponse> GetStarshipsAsync(string next = "");
    Task<StarshipResponse> GetStarshipAsync(string id);
}

public class SwapiClient : ISwapiClient
{
    private readonly HttpClient _httpClient;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public SwapiClient(IHttpClientFactory httpClientFactory, IOptions<SwapiSettings> swapiOptions)
    {
        var swapiSettings = swapiOptions.Value ?? throw new ArgumentNullException(nameof(swapiOptions), "Swapi settings cannot be null");

        if(string.IsNullOrEmpty(swapiSettings.BaseUrl))
            throw new ApplicationException("Swapi base URL cannot be null or empty");

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(swapiSettings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        _retryPolicy = Policy
        .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .WaitAndRetryAsync(
            swapiSettings.RetryCount,
            retryAttempt => TimeSpan.FromSeconds(swapiSettings.RetryDelaySeconds));
    }

    public async Task<StarshipPageResponse> GetStarshipsAsync(string next = "")
    {
        var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(next));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var starships = JsonSerializer.Deserialize<StarshipPageResponse>(content)
            ?? throw new ApplicationException("Falied to deserialize starships");

        return starships;
    }


    public async Task<StarshipResponse> GetStarshipAsync(string id)
    {
        var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync($"starships/{id}"));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var starships = JsonSerializer.Deserialize<StarshipResponse>(content)
            ?? throw new ApplicationException("Falied to deserialize starships");

        return starships;
    }
}
