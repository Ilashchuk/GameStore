using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IGameRepository Games { get; }

    IGenreRepository Genres { get; }

    IPlatformRepository Platforms { get; }

    void Complete();

    Task CompleteAsync();
}
