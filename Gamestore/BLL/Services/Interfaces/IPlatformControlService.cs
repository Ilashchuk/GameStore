using BLL.Models;

namespace BLL.Services.Interfaces;
public interface IPlatformControlService : IControlService<PlatformDto>
{
    Task<IEnumerable<GameDto>?> GetGamesAsync(Guid id);
}
