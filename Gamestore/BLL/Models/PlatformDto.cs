namespace BLL.Models;
public class PlatformDto : BaseDto
{
    public string Type { get; set; }

    public ICollection<GameDto> GamesDto { get; set; }
}
