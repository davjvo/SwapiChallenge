using Microsoft.AspNetCore.Mvc;
using StarWars.Domain.DTOs;
using StarWars.Domain.Interfaces.Security;

namespace StarWars.API.Controllers;

[ApiController, Route("api/[controller]")]
public class AuthController(ITokenService tokenService, IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var loginResponse = new LoginResponse();
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            loginResponse.Message = "Username and password are required.";
            return BadRequest(loginResponse);
        }

        var user = await authService.AuthenticateAsync(request.Username, request.Password);
        if (user == null)
        {
            loginResponse.Message = "Invalid credentials.";
            return Unauthorized(loginResponse);
        }

        loginResponse.Success = true;
        loginResponse.Token = tokenService.CreateToken(user);
        return Ok(loginResponse);
    }
}
