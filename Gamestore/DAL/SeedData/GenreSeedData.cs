using System.Diagnostics.CodeAnalysis;
using DAL.Entities;

namespace DAL.SeedData;

[ExcludeFromCodeCoverage]
public static class GenreSeedData
{
    public static List<Genre> GetGenres()
    {
        var strategyId = Guid.NewGuid();
        var rtsId = Guid.NewGuid();
        var tbsId = Guid.NewGuid();

        var rpgId = Guid.NewGuid();

        var sportsId = Guid.NewGuid();
        var racesId = Guid.NewGuid();
        var rallyId = Guid.NewGuid();
        var arcadeId = Guid.NewGuid();
        var formulaId = Guid.NewGuid();
        var offRoadId = Guid.NewGuid();

        var actionId = Guid.NewGuid();
        var fpsId = Guid.NewGuid();
        var tpsId = Guid.NewGuid();

        var adventureId = Guid.NewGuid();
        var puzzleId = Guid.NewGuid();

        return
        [
            new Genre { Id = strategyId, Name = "Strategy", ParentGenreId = null },
            new Genre { Id = rtsId, Name = "RTS", ParentGenreId = strategyId },
            new Genre { Id = tbsId, Name = "TBS", ParentGenreId = strategyId },

            new Genre { Id = rpgId, Name = "RPG", ParentGenreId = null },

            new Genre { Id = sportsId, Name = "Sports", ParentGenreId = null },
            new Genre { Id = racesId, Name = "Races", ParentGenreId = sportsId },
            new Genre { Id = rallyId, Name = "Rally", ParentGenreId = racesId },
            new Genre { Id = arcadeId, Name = "Arcade", ParentGenreId = racesId },
            new Genre { Id = formulaId, Name = "Formula", ParentGenreId = racesId },
            new Genre { Id = offRoadId, Name = "Off-road", ParentGenreId = racesId },

            new Genre { Id = actionId, Name = "Action", ParentGenreId = null },
            new Genre { Id = fpsId, Name = "FPS", ParentGenreId = actionId },
            new Genre { Id = tpsId, Name = "TPS", ParentGenreId = actionId },

            new Genre { Id = adventureId, Name = "Adventure", ParentGenreId = null },
            new Genre { Id = puzzleId, Name = "Puzzle & Skill", ParentGenreId = null },
        ];
    }
}
