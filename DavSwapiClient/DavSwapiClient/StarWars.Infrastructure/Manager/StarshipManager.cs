using Microsoft.Extensions.Options;
using StarWars.Domain.Config;
using StarWars.Domain.DTOs.External;
using StarWars.Domain.Interfaces.Caching;
using StarWars.Domain.Interfaces.Manager;
using StarWars.Infrastructure.Caching;
using StarWars.Infrastructure.External;

namespace StarWars.Infrastructure.Manager;

public class StarshipManager(ICacheService cache, ISwapiClient swapiClient, IOptions<SwapiSettings> options) : IStarshipManager
{
    private const string StarshipsEndpoint = "starships/";
    private readonly string _baseUrl = options.Value?.BaseUrl ?? throw new ArgumentNullException(nameof(options), "SwapiClient base URL cannot be null");

    public async Task<List<StarshipSummary>> GetAllAsync()
    {
        return await cache.GetOrCreateAsync(MemoryCacheService.Starships, async () =>
        {
            var starships = new List<StarshipSummary>();
            var manufacturers = new Dictionary<string, List<StarshipSummary>>();
            var nextUrl = StarshipsEndpoint;

            while (!string.IsNullOrEmpty(nextUrl))
            {
                var currentPage = await swapiClient.GetStarshipsAsync(nextUrl);
                nextUrl = currentPage.Next?.Replace(_baseUrl, "");

                foreach (var summary in currentPage.Starships ?? [])
                {
                    if (string.IsNullOrEmpty(summary.Id))
                        continue;

                    var starship = await swapiClient.GetStarshipAsync(summary.Id);
                    var detail = starship.Result?.Starship;
                    if (detail is null)
                        continue;

                    // uid is one level above in the api structure, to avoid doble declaration just assign here since I already have the information
                    detail.Id = summary.Id;
                    starships.Add(detail);

                    var rawManufacturers = detail.Manufacturer ?? "Unknown";
                    var manufacturerList = rawManufacturers
                        .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Replace("/", "-"));

                    foreach (var manufacturer in manufacturerList)
                    {
                        if (!manufacturers.TryGetValue(manufacturer, out var items))
                        {
                            items = [];
                            manufacturers[manufacturer] = items;
                        }

                        items.Add(detail);
                    }
                }
            }

            await cache.SetAsync(MemoryCacheService.Manufacturers, manufacturers, TimeSpan.FromMinutes(60));
            return starships;
        });
    }

    public async Task<List<StarshipSummary>> GetByManufacturerAsync(string name)
    {
        if (string.IsNullOrEmpty(name)) return await GetAllAsync();

        var manufacturers = cache.Get<Dictionary<string, List<StarshipSummary>>>(MemoryCacheService.Manufacturers);

        if (manufacturers == null)
        {
            await GetAllAsync(); // Rehydrates manufacturers too
            manufacturers = cache.Get<Dictionary<string, List<StarshipSummary>>>(MemoryCacheService.Manufacturers);
        }

        if (manufacturers == null || !manufacturers.TryGetValue(name, out var starships))
            return [];

        return starships;
    }

    public async Task<IEnumerable<string>> GetManufacturers()
    {
        var manufacturers = cache.Get<Dictionary<string, List<StarshipSummary>>>(MemoryCacheService.Manufacturers);

        if (manufacturers == null)
        {
            await GetAllAsync(); // Rehydrates manufacturers too
            manufacturers = cache.Get<Dictionary<string, List<StarshipSummary>>>(MemoryCacheService.Manufacturers);
        }

        if (manufacturers == null)
            return [];

        return manufacturers.Keys;
    }
}
