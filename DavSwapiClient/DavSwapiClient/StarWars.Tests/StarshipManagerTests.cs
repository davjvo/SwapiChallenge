using Microsoft.Extensions.Options;
using StarWars.Domain.Config;
using StarWars.Infrastructure.Manager;
using StarWars.Tests.Mocks;
using Bogus;

namespace StarWars.Tests;

[TestClass]
public class StarshipManagerTests
{
    private StarshipManager _manager;
    private readonly Faker _faker = new();

    [TestInitialize]
    public void Setup()
    {
        var cacheMock = new CacheServiceMock();
        var swapiClientMock = new SwapiClientMock();
        var settings = Options.Create(new SwapiSettings
        {
            BaseUrl = _faker.Random.String(), 
            RetryCount = 3, 
            RetryDelaySeconds = 1, 
            TimeoutSeconds = 100
        });
        _manager = new StarshipManager(cacheMock, swapiClientMock, settings);
    }
    
    [TestMethod]
    public async Task GetAllAsync_ShouldReturnData()
    {
        var data = await _manager.GetAllAsync();
        
        Assert.IsNotNull(data);
    }

    [TestMethod]
    public async Task GetManufacturers_ShouldReturnData()
    {
        var data = await _manager.GetManufacturers();
        
        Assert.IsNotNull(data);
    }
}
