using Newtonsoft.Json;

namespace Gamestore.Models.RequestViewModels;

public class AddUpdateGenreViewModel
{
    [JsonProperty("genre")]
    public GenreViewModel Genre { get; set; }
}
