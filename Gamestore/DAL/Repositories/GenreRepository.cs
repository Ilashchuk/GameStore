using System.Diagnostics.CodeAnalysis;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

[ExcludeFromCodeCoverage]
public class GenreRepository : Repository<Genre>, IGenreRepository
{
    private readonly GameContext _dbContext;

    public GenreRepository(GameContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<Genre?> GetByIdAsync(Guid id, bool tracking)
    {
        return tracking
            ? await _dbContext.Genres
                                   .Include(x => x.Games)
                                   .FirstOrDefaultAsync(x => x.Id == id)
            : await _dbContext.Genres
                                   .AsNoTracking()
                                   .Include(x => x.Games)
                                   .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Genre>> GetSubGenresAsync(Guid parentGenreId)
    {
        return await _dbContext.Genres.Where(x => x.ParentGenreId == parentGenreId).ToListAsync();
    }
}