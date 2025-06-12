using StarWars.Domain.Models;

namespace StarWars.Domain.Interfaces.Security;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string username, string password);
}
