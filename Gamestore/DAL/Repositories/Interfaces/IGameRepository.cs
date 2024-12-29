using DAL.Entities;

namespace DAL.Repositories.Interfaces;
public interface IGameRepository : IRepository<Game>
{
    Task<Game?> GetByKeyAsync(string key);

    Task<bool> DeleteByKeyAsync(string key);

    Task<int> GetCountAsync();

    Task UpdateAsync(Game entity);
}
