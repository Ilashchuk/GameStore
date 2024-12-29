using System.Diagnostics.CodeAnalysis;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

[ExcludeFromCodeCoverage]
public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(GameContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>?> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool tracking)
    {
        return tracking ? await _dbSet.FirstOrDefaultAsync(e => e.Id == id) : await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Local.Clear();

        _dbSet.Update(entity);
    }

    public virtual async Task<bool> DeleteByIdAsync(Guid id)
    {
        var count = await _dbSet.Where(e => e.Id == id).ExecuteDeleteAsync();

        return count > 0;
    }
}
