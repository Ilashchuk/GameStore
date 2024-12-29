namespace BLL.Models;
public class GenreDto : BaseDto
{
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }

    public GenreDto? ParentGenre { get; set; }

    public ICollection<GameDto> GamesDto { get; set; }

    public ICollection<GenreDto> ChildGenresDto { get; set; }
}
