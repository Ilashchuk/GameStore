using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;
public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);

        builder
            .HasIndex(g => g.Name)
            .IsUnique();

        builder
            .Property(g => g.Name)
            .IsRequired();

        builder
            .HasMany(g => g.Games)
            .WithMany(g => g.Genres);

        builder
            .HasMany(g => g.ChildGenres)
            .WithOne(g2 => g2.ParentGenre);

        builder
            .HasOne(g => g.ParentGenre)
            .WithMany(g2 => g2.ChildGenres)
            .HasForeignKey(g2 => g2.ParentGenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
