using System.Diagnostics.CodeAnalysis;
using DAL.Configurations;
using DAL.Entities;
using DAL.SeedData;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

[ExcludeFromCodeCoverage]
public class GameContext : DbContext
{
    public GameContext(DbContextOptions<GameContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; } = null!;

    public DbSet<Genre> Genres { get; set; } = null!;

    public DbSet<Platform> Platforms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());
        modelBuilder.ApplyConfiguration(new PlatformConfiguration());

        modelBuilder.Entity<Genre>().HasData(GenreSeedData.GetGenres());

        base.OnModelCreating(modelBuilder);
    }
}
