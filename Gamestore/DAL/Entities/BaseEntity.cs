using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Entities;

[ExcludeFromCodeCoverage]
public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
