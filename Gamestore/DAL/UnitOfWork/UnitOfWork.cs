using System.Diagnostics.CodeAnalysis;
using DAL.Data;
using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork;

[ExcludeFromCodeCoverage]
public class UnitOfWork : IUnitOfWork
{
    private readonly GameContext _context;
    private bool _disposed;

    public UnitOfWork(GameContext context, IGameRepository gameRepository, IGenreRepository genreRepository, IPlatformRepository platformRepository)
    {
        _disposed = false;

        _context = context;

        Games = gameRepository;

        Genres = genreRepository;

        Platforms = platformRepository;
    }

    public IGameRepository Games { get; private set; }

    public IGenreRepository Genres { get; private set; }

    public IPlatformRepository Platforms { get; private set; }

    public void Complete()
    {
        _context.SaveChanges();
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
