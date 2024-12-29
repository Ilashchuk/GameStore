using BLL.Models;

namespace BLL.Services.Interfaces;
public interface IGameControlService : IControlService<GameDto>
{
    Task<GameDto?> GetByKeyAsync(string key);

    Task<bool> RemoveByKeyAsync(string key);

    Task<IEnumerable<GenreDto?>> GetGenresAsync(string key);

    Task<IEnumerable<PlatformDto?>> GetPlatformsAsync(string key);

    Task<byte[]> GenerateGameFileAsync(string key);

    Task<int> GetTotalGamesCountAsync();
}
