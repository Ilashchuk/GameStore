using DAL.Entities;

namespace DAL.Repositories.Interfaces;
public interface IGenreRepository : IRepository<Genre>
{
    Task<List<Genre>> GetSubGenresAsync(Guid parentGenreId);
}
