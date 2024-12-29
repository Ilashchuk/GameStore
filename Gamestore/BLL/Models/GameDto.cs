using Newtonsoft.Json;

namespace BLL.Models;
[JsonObject("game")]
public class GameDto : BaseDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonIgnore]
    public ICollection<GenreDto> GenresDto { get; set; }

    [JsonIgnore]
    public ICollection<PlatformDto> PlatformsDto { get; set; }
}
