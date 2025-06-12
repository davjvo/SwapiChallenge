using StarWars.Domain.DTOs.External;
using StarWars.Infrastructure.External;
using Bogus;

namespace StarWars.Tests.Mocks;

public class SwapiClientMock : ISwapiClient
{
    private readonly Faker<StarshipSummary> _starshipSummaryFaker = new Faker<StarshipSummary>()
        .RuleFor(s => s.Name, f => f.Person.FullName)
        .RuleFor(s => s.Manufacturer, f => f.Company.CompanyName())
        .RuleFor(s => s.Id, f => new Faker().Random.String(4));
    
    public async Task<StarshipPageResponse> GetStarshipsAsync(string next = "")
    {
        var response = new StarshipPageResponse
        {
            Next = "",
            Starships = _starshipSummaryFaker.Generate(10),
        };
        return await Task.FromResult(response);
    }

    public async Task<StarshipResponse> GetStarshipAsync(string id)
    {
        var response = new StarshipResponse
        {
            Result = new StarshipResponse.StarshipResult
            {
                Starship = _starshipSummaryFaker.Generate()
            }
        };
        return await Task.FromResult(response);
    }
}