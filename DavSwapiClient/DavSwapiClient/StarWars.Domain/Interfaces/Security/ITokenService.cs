using StarWars.Domain.Models;

namespace StarWars.Domain.Interfaces.Security;

public interface ITokenService
{
    string CreateToken(User user);
}
