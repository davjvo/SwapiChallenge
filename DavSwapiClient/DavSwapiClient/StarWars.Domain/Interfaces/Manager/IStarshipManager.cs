using StarWars.Domain.DTOs.External;

namespace StarWars.Domain.Interfaces.Manager;

public interface IStarshipManager
{
    Task<List<StarshipSummary>> GetAllAsync();
    Task<IEnumerable<string>> GetManufacturers();
    Task<List<StarshipSummary>> GetByManufacturerAsync(string name);
}
