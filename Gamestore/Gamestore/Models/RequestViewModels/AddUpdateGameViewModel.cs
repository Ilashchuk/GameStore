using Newtonsoft.Json;

namespace Gamestore.Models.RequestViewModels;

public class AddUpdateGameViewModel
{
    [JsonProperty("game")]
    public GameViewModel Game { get; set; }

    [JsonProperty("genres")]
    public ICollection<Guid> GenreIds { get; set; }

    [JsonProperty("platforms")]
    public ICollection<Guid> PlatformIds { get; set; }
}
