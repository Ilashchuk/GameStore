using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gamestore.Models;

[JsonObject("platform")]
public class PlatformViewModel
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("type")]
    [Required]
    public string Type { get; set; }
}
