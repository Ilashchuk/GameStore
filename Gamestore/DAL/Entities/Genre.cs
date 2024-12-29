using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Entities;

[ExcludeFromCodeCoverage]
public class Genre : BaseEntity
{
    [Required]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }

    public Genre? ParentGenre { get; set; }

    public ICollection<Game> Games { get; set; }

    public ICollection<Genre> ChildGenres { get; set; }
}