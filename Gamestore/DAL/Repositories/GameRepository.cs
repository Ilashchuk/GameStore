using System.Diagnostics.CodeAnalysis;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

[ExcludeFromCodeCoverage]
public class GameRepository : Repository<Game>, IGameRepository
{
    private readonly GameContext _context;

    public GameRepository(GameContext dbContext)
        : base(dbContext)
    {
        _context = dbContext;
    }

    public override async Task AddAsync(Game? entity)
    {
        var genreIds = entity.Genres.Select(x => x.Id).ToList();
        var platformIds = entity.Platforms.Select(x => x.Id).ToList();

        var genres = await _context.Genres.Where(x => genreIds.Contains(x.Id)).ToListAsync();
        var platforms = await _context.Platforms.Where(x => platformIds.Contains(x.Id)).ToListAsync();

        entity.Genres = genres;
        entity.Platforms = platforms;

        await _context.Games.AddAsync(entity);
    }

    public override async Task<Game?> GetByIdAsync(Guid id, bool tracking)
    {
        return tracking ? await _context.Games
            .Include(x => x.Genres)
            .Include(x => x.Platforms)
            .FirstOrDefaultAsync(x => x.Id == id)
            : await _context.Games
            .Include(x => x.Genres)
            .Include(x => x.Platforms)
            .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Game?> GetByKeyAsync(string key)
    {
        return await _context.Games
            .Include(x => x.Genres)
            .Include(x => x.Platforms)
            .FirstOrDefaultAsync(x => x.Key == key);
    }

    public async Task UpdateAsync(Game entity)
    {
        Game? gameFromDb = await GetByIdAsync(entity.Id, true);

        gameFromDb.Name = entity.Name;
        gameFromDb.Key = entity.Key;
        gameFromDb.Description = entity.Description;

        await UpdateRelationsAsync(gameFromDb.Genres, entity.Genres.Select(x => x.Id), _context.Genres);
        await UpdateRelationsAsync(gameFromDb.Platforms, entity.Platforms.Select(x => x.Id), _context.Platforms);

        _context.Entry(gameFromDb).Collection(g => g.Genres).IsModified = true;
        _context.Entry(gameFromDb).Collection(p => p.Platforms).IsModified = true;

        _context.Entry(gameFromDb).State = EntityState.Modified;
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        var count = await _context.Games.Where(e => e.Key == key).ExecuteDeleteAsync();

        return count > 0;
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Games.CountAsync();
    }

    private static async Task UpdateRelationsAsync<T>(ICollection<T> existingItems, IEnumerable<Guid> updatedIds, DbSet<T> dbSet)
        where T : BaseEntity
    {
        var existingIds = existingItems.Select(x => x.Id).ToList();
        var idsToRemove = existingIds.Except(updatedIds).ToList();
        var idsToAdd = updatedIds.Except(existingIds).ToList();

        if (idsToRemove.Count != 0)
        {
            var itemsToRemove = existingItems.Where(x => idsToRemove.Contains(x.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                existingItems.Remove(item);
            }
        }

        if (idsToAdd.Count != 0)
        {
            var itemsToAdd = await dbSet.Where(x => idsToAdd.Contains(x.Id)).ToListAsync();
            foreach (var item in itemsToAdd)
            {
                existingItems.Add(item);
            }
        }
    }
}
