using DAL.Entities;

namespace DAL.Repositories.Interfaces;
public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<List<TEntity>?> GetAllAsync();

    Task<TEntity?> GetByIdAsync(Guid id, bool tracking);

    Task AddAsync(TEntity entity);

    Task<bool> DeleteByIdAsync(Guid id);

    void Update(TEntity entity);
}
