using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Entities;

[ExcludeFromCodeCoverage]
public class Platform : BaseEntity
{
    [Required]
    public string Type { get; set; }

    public ICollection<Game> Games { get; set; }
}