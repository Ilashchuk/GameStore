using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;
public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasKey(g => g.Id);

        builder
            .HasIndex(g => g.Key)
            .IsUnique();

        builder
            .Property(g => g.Name)
            .IsRequired();

        builder
            .Property(g => g.Key)
            .IsRequired();

        builder
            .HasMany(g => g.Genres)
            .WithMany(g => g.Games);

        builder
            .HasMany(g => g.Platforms)
            .WithMany(g => g.Games);
    }
}
