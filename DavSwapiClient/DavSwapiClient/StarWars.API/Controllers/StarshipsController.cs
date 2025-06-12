using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWars.Domain.Interfaces.Manager;

namespace StarWars.API.Controllers;

[ApiController, Route("api/[controller]"), Authorize]
public class StarshipsController(IStarshipManager starshipManager) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetStarshipsAsync()
    {
        var result = await starshipManager.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetStarshipsAsync([FromRoute] string name)
    {
        var result = await starshipManager.GetByManufacturerAsync(name);
        return Ok(result);
    }

    [HttpGet("manufacturers")]
    public async Task<IActionResult> GetManufacturers()
    {
        var result = await starshipManager.GetManufacturers();
        return Ok(result);
    }
}
