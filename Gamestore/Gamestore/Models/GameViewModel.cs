using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gamestore.Models;

[JsonObject("game")]
public class GameViewModel
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    [Required]
    public string Name { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }
}