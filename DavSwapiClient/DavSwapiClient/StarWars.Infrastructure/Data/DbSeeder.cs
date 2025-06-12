using Microsoft.AspNetCore.Identity;
using StarWars.Domain.Models;

namespace StarWars.Infrastructure.Data;

public static class DbSeeder
{
    public static void SeedDefaultUser(StarWarsDbContext context)
    {
        var existingUser = context.Users.FirstOrDefault(u => u.Username == "admin");
        if (existingUser != null) return;

        var user = new User
        {
            Username = "admin",
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            ModifiedAt = DateTime.UtcNow
        };

        user.Password = new PasswordHasher<User>().HashPassword(user, "admin");

        context.Users.Add(user);
        context.SaveChanges();
    }
}
