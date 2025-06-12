using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StarWars.Domain.Config;
using StarWars.Domain.Interfaces.Security;
using StarWars.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StarWars.Infrastructure.Services.Security;

public class TokenService(IOptions<JwtSettings> jwtOptions) : ITokenService
{
    private readonly JwtSettings settings = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions), "Jwt settings cannot be null");

    public string CreateToken(User user)
    {

        if(string.IsNullOrEmpty(user.Username)) throw new ApplicationException("Username cannot be null or empty.");

        if(string.IsNullOrEmpty(settings.Key)) throw new ApplicationException("Jwt key cannot be null or empty.");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
