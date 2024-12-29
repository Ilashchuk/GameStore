using Newtonsoft.Json;

namespace BLL.Models;
public class BaseDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
}
