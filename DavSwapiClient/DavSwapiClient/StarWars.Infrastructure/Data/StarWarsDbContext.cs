using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Models;

namespace StarWars.Infrastructure.Data;

public class StarWarsDbContext(DbContextOptions<StarWarsDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
