using Newtonsoft.Json;

namespace Gamestore.Models.RequestViewModels;

public class AddUpdatePlatformViewModel
{
    [JsonProperty("platform")]
    public PlatformViewModel Platform { get; set; }
}
