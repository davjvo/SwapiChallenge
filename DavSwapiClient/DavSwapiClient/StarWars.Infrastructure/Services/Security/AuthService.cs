using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Interfaces.Security;
using StarWars.Domain.Models;
using StarWars.Infrastructure.Data;

namespace StarWars.Infrastructure.Services.Security;

public class AuthService(StarWarsDbContext context) : IAuthService
{
    private readonly PasswordHasher<User> _hasher = new();

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || string.IsNullOrEmpty(user.Password)) return null;

        var result = _hasher.VerifyHashedPassword(user, user.Password, password);
        if(result != PasswordVerificationResult.Success) return null;

        return user;
    }
}
