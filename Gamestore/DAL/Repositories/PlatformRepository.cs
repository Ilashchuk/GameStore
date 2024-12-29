using System.Diagnostics.CodeAnalysis;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

[ExcludeFromCodeCoverage]
public class PlatformRepository : Repository<Platform>, IPlatformRepository
{
    private readonly GameContext _dbContext;

    public PlatformRepository(GameContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<Platform?> GetByIdAsync(Guid id, bool tracking)
    {
        return tracking
            ? await _dbContext.Platforms
                                   .Include(x => x.Games)
                                   .FirstOrDefaultAsync(x => x.Id == id)
            : await _dbContext.Platforms
                                   .AsNoTracking()
                                   .Include(x => x.Games)
                                   .FirstOrDefaultAsync(x => x.Id == id);
    }
}
