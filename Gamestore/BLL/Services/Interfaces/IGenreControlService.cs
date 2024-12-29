using BLL.Models;

namespace BLL.Services.Interfaces;
public interface IGenreControlService : IControlService<GenreDto>
{
    Task<IEnumerable<GameDto>?> GetGamesAsync(Guid id);

    Task<IEnumerable<GenreDto?>> GetSubGenresAsync(Guid parentId);
}
