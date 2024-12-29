using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gamestore.Models;

[JsonObject("genre")]
public class GenreViewModel
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    [Required]
    public string Name { get; set; }

    [JsonProperty("parentGenreId")]
    public Guid? ParentGenreId { get; set; }
}
