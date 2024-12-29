using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Entities;

[ExcludeFromCodeCoverage]

public class Game : BaseEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Key { get; set; }

    public string? Description { get; set; }

    public ICollection<Genre> Genres { get; set; }

    public ICollection<Platform> Platforms { get; set; }
}
