using BLL.Models;
using DAL.Entities;
using Gamestore.Models;
using Gamestore.Models.RequestViewModels;

namespace Gamestore.Tests.Data;

public static class TestData
{
    private static readonly Guid TestGameId = Guid.NewGuid();
    private static readonly Guid AnotherTestGameId = Guid.NewGuid();
    private static readonly Guid StrategyId = Guid.NewGuid();
    private static readonly Guid RTSId = Guid.NewGuid();
    private static readonly Guid PCId = Guid.NewGuid();
    private static readonly Guid ConsoleId = Guid.NewGuid();

    public static GameViewModel GetTestGameViewModel()
    {
        return new GameViewModel
        {
            Id = TestGameId,
            Name = "Test Game",
            Key = "test-game-key",
            Description = "A sample game for testing",
        };
    }

    public static GameDto GetTestGameDTO()
    {
        return new GameDto
        {
            Id = TestGameId,
            Name = "Test Game",
            Key = "test-game-key",
            Description = "A sample game for testing",
            GenresDto = GetTestGenresDTO(),
            PlatformsDto = GetTestPlatformsDTO(),
        };
    }

    public static Game GetTestGame()
    {
        return new Game
        {
            Id = TestGameId,
            Name = "Test Game",
            Key = "test-game-key",
            Description = "A sample game for testing",
            Genres = GetTestGenres(),
            Platforms = GetTestPlatforms(),
        };
    }

    public static List<GameViewModel> GetTestGamesViewModel()
    {
        return
        [
            GetTestGameViewModel(),
            new GameViewModel
            {
                Id = AnotherTestGameId,
                Name = "Another Test Game",
                Key = "another-test-key",
                Description = "Another sample game",
            },
        ];
    }

    public static List<GameDto> GetTestGamesDTO()
    {
        return
        [
            GetTestGameDTO(),
            new GameDto
            {
                Id = AnotherTestGameId,
                Name = "Another Test Game",
                Key = "another-test-key",
                Description = "Another sample game",
                GenresDto =
                    [
                        new()
                        {
                            Id = StrategyId,
                            Name = "Strategy",
                            ParentGenreId = null,
                        },
                    ],
                PlatformsDto = GetTestPlatformsDTO(),
            },
        ];
    }

    public static List<Game> GetTestGames()
    {
        return
        [
            GetTestGame(),
            new Game
            {
                Id = AnotherTestGameId,
                Name = "Another Test Game",
                Key = "another-test-key",
                Description = "Another sample game",
                Genres =
                    [
                        new()
                        {
                            Id = StrategyId,
                            Name = "Strategy",
                            ParentGenreId = null,
                        },
                    ],
                Platforms = GetTestPlatforms(),
            },
        ];
    }

    public static GenreViewModel GetTestGenreViewModel()
    {
        return new GenreViewModel
        {
            Id = StrategyId,
            Name = "Strategy",
            ParentGenreId = null,
        };
    }

    public static GenreDto GetTestGenreDTO()
    {
        return new GenreDto
        {
            Id = StrategyId,
            Name = "Strategy",
            ParentGenreId = null,
        };
    }

    public static Genre GetTestGenre()
    {
        return new Genre
        {
            Id = StrategyId,
            Name = "Strategy",
            ParentGenreId = null,
        };
    }

    public static List<GenreViewModel> GetTestGenresViewModel()
    {
        return
        [
            new GenreViewModel
            {
                Id = StrategyId,
                Name = "Strategy",
                ParentGenreId = null,
            },
            new GenreViewModel
            {
                Id = RTSId,
                Name = "RTS",
                ParentGenreId = StrategyId,
            },
        ];
    }

    public static List<GenreDto> GetTestGenresDTO()
    {
        return
        [
            new GenreDto
            {
                Id = StrategyId,
                Name = "Strategy",
                ParentGenreId = null,
            },
            new GenreDto
            {
                Id = RTSId,
                Name = "RTS",
                ParentGenreId = StrategyId,
            },
        ];
    }

    public static List<Genre> GetTestGenres()
    {
        return
        [
            new Genre
            {
                Id = StrategyId,
                Name = "Strategy",
                ParentGenreId = null,
            },
            new Genre
            {
                Id = RTSId,
                Name = "RTS",
                ParentGenreId = StrategyId,
            },
        ];
    }

    public static List<PlatformViewModel> GetTestPlatformsViewModel()
    {
        return
        [
            new PlatformViewModel
            {
                Id = PCId,
                Type = "PC",
            },
            new PlatformViewModel
            {
                Id = ConsoleId,
                Type = "Console",
            },
        ];
    }

    public static List<PlatformDto> GetTestPlatformsDTO()
    {
        return
        [
            new PlatformDto
            {
                Id = PCId,
                Type = "PC",
            },
            new PlatformDto
            {
                Id = ConsoleId,
                Type = "Console",
            },
        ];
    }

    public static List<Platform> GetTestPlatforms()
    {
        return
        [
            new Platform
            {
                Id = PCId,
                Type = "PC",
            },
            new Platform
            {
                Id = ConsoleId,
                Type = "Console",
            },
        ];
    }

    public static Platform GetTestPlatform()
    {
        return
            new Platform
            {
                Id = PCId,
                Type = "PC",
            };
    }

    public static PlatformDto GetTestPlatformDTO()
    {
        return
            new PlatformDto
            {
                Id = PCId,
                Type = "PC",
            };
    }

    public static PlatformViewModel GetTestPlatformViewModel()
    {
        return
            new PlatformViewModel
            {
                Id = PCId,
                Type = "PC",
            };
    }

    public static AddUpdateGameViewModel GetTestAddUpdateTestGameViewModel()
    {
        return
        new AddUpdateGameViewModel
        {
            Game = GetTestGameViewModel(),
            GenreIds = GetTestGenresViewModel().Select(x => x.Id).ToList(),
            PlatformIds = GetTestPlatformsViewModel().Select(x => x.Id).ToList(),
        };
    }

    public static AddUpdateGenreViewModel GetTestAddUpdateTestGenreViewModel()
    {
        return
        new AddUpdateGenreViewModel
        {
            Genre = GetTestGenreViewModel(),
        };
    }

    public static AddUpdatePlatformViewModel GetTestAddUpdateTestPlatformViewModel()
    {
        return
        new AddUpdatePlatformViewModel
        {
            Platform = GetTestPlatformViewModel(),
        };
    }
}
