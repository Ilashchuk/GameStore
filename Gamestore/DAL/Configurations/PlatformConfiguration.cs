using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;
public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder
            .HasKey(g => g.Id);

        builder
            .HasIndex(g => g.Type)
            .IsUnique();

        builder
            .Property(p => p.Type)
            .IsRequired();

        builder
            .HasMany(p => p.Games)
            .WithMany(g => g.Platforms);
    }
}
