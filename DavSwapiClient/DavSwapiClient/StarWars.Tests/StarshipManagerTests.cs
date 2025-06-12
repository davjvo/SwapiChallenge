using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StarWars.Domain.Config;
using StarWars.Domain.DTOs.External;
using StarWars.Domain.Interfaces.Caching;
using StarWars.Infrastructure.Caching;
using StarWars.Infrastructure.External;
using StarWars.Infrastructure.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars.Tests;

[TestClass]
public class StarshipManagerTests
{
    private Mock<ICacheService> _cacheMock;
    private Mock<ISwapiClient> _swapiClientMock;
    private IOptions<SwapiSettings> _settings;
    private StarshipManager _manager;


    [TestInitialize]
    public void Setup()
    {
        _cacheMock = new Mock<ICacheService>();
        _swapiClientMock = new Mock<ISwapiClient>();
        _settings = Options.Create(new SwapiSettings { BaseUrl = "https://swapi.dev/api/", RetryCount = 3, RetryDelaySeconds = 1 });
        _manager = new StarshipManager(_cacheMock.Object, _swapiClientMock.Object, _settings);
    }
}
