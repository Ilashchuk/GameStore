using System.Text;
using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.UnitOfWork;
using Gamestore.Tests.Data;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Gamestore.Tests.ServiceTests;

public class GameControlServiceTests : IDisposable
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GameControlService _service;
    private readonly Mock<ILogger<GameControlService>> _mockLogger;

    public GameControlServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GameControlService>>();
        _service = new GameControlService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GivenGamesExist_WhenGetAllAsyncIsCalled_ThenReturnsList()
    {
        // Arrange
        var testGames = TestData.GetTestGames();
        _mockUnitOfWork.Setup(uow => uow.Games.GetAllAsync()).ReturnsAsync(testGames);
        _mockMapper.Setup(m => m.Map<IEnumerable<GameDto>>(It.IsAny<IEnumerable<Game>>()))
                   .Returns(TestData.GetTestGamesDTO());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGames.Count, result.Count());
    }

    [Fact]
    public async Task GivenNoGamesExist_WhenGetAllAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGames = TestData.GetTestGames();

        _mockUnitOfWork.Setup(uow => uow.Games.GetAllAsync()).ReturnsAsync((List<Game>?)null);
        _mockMapper.Setup(m => m.Map<IEnumerable<GameDto>>(It.IsAny<IEnumerable<Game>>()))
                   .Returns(TestData.GetTestGamesDTO());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameExists_WhenGetByIdAsyncIsCalled_ThenReturnsGame()
    {
        // Arrange
        var testGame = TestData.GetTestGame();
        var testGameDto = TestData.GetTestGameDTO();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync(testGame);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(testGameDto);

        // Act
        var result = await _service.GetByIdAsync(testGame.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGame.Id, result.Id);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenGetByIdAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGame = TestData.GetTestGame();
        var testGameDto = TestData.GetTestGameDTO();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Game)null);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(testGameDto);

        // Act
        var result = await _service.GetByIdAsync(testGame.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenGetByIdAsyncIsCalled_ThenNullIsReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Games.GetByIdAsync(id, true)).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameExists_WhenGetByKeyAsyncIsCalled_ThenReturnsGame()
    {
        // Arrange
        var testGame = TestData.GetTestGames().First();
        var testGameDto = TestData.GetTestGamesDTO().First();
        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync(testGame);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(testGameDto);

        // Act
        var result = await _service.GetByKeyAsync(testGame.Key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGame.Key, result.Key);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenGetByKeyAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGame = TestData.GetTestGames().First();
        var testGameDto = TestData.GetTestGamesDTO().First();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync((Game)null);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(testGameDto);

        // Act
        var result = await _service.GetByKeyAsync(testGame.Key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameWithKeyDoesNotExist_WhenGetByKeyAsyncIsCalled_ThenNullIsReturned()
    {
        // Arrange
        var key = "non-existent-key";
        _mockUnitOfWork.Setup(u => u.Games.GetByKeyAsync(key)).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GetByKeyAsync(key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenValidGameDto_WhenCreateAsyncIsCalled_ThenReturnsCreatedGame()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();
        var newGame = TestData.GetTestGame();

        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(newGame);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(newGameDto);
        _mockUnitOfWork.Setup(uow => uow.Games.AddAsync(It.IsAny<Game>()));

        // Act
        var result = await _service.CreateAsync(newGameDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newGameDto.Name, result.Name);
    }

    [Fact]
    public async Task GivenGameDtoWithoutKey_WhenCreateAsyncIsCalled_ThenGeneratesKey()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();
        newGameDto.Key = null;
        var newGame = TestData.GetTestGame();

        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(newGame);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(newGameDto);
        _mockUnitOfWork.Setup(uow => uow.Games.AddAsync(It.IsAny<Game>()));

        // Act
        var result = await _service.CreateAsync(newGameDto);

        // Assert
        Assert.NotNull(result.Key);
        Assert.StartsWith(newGameDto.Name.ToLower(System.Globalization.CultureInfo.CurrentCulture), result.Key);
    }

    [Fact]
    public async Task GivenValidGameDto_WhenCreateAsyncIsCalled_ThenCallsAddAndComplete()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();
        var newGame = TestData.GetTestGame();

        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(newGame);
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(newGameDto);
        _mockUnitOfWork.Setup(uow => uow.Games.AddAsync(It.IsAny<Game>()));

        // Act
        await _service.CreateAsync(newGameDto);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Games.AddAsync(It.IsAny<Game>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenNullGameDto_WhenCreateAsyncIsCalled_ThenThrowsArgumentNullException()
    {
        // Arrange
        GameDto? newGameDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(newGameDto!));
    }

    [Fact]
    public async Task GivenValidGameDto_WhenCompleteAsyncFails_ThenThrowsException()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();
        var newGame = TestData.GetTestGame();

        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(newGame);
        _mockUnitOfWork.Setup(uow => uow.Games.AddAsync(It.IsAny<Game>()));
        _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(newGameDto));
        Assert.Equal("Database error", exception.Message);
    }

    [Fact]
    public async Task GivenGameDtoWithEmptyName_WhenCreateAsyncIsCalled_ThenThrowsArgumentException()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();
        newGameDto.Name = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(newGameDto));
    }

    [Fact]
    public async Task GivenMapperFailsToMap_WhenCreateAsyncIsCalled_ThenThrowsException()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();

        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>()))
                   .Throws(new InvalidOperationException("Mapping failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(newGameDto));
        Assert.Equal("Mapping failed", exception.Message);
    }

    [Fact]
    public async Task GivenExceptionOccursInAddAsync_WhenCreateAsyncIsCalled_ThenDoesNotCallCompleteAsync()
    {
        // Arrange
        var newGameDto = TestData.GetTestGameDTO();

        _mockUnitOfWork.Setup(uow => uow.Games.AddAsync(It.IsAny<Game>()))
                       .ThrowsAsync(new Exception("AddAsync failed"));

        // Act
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(newGameDto));

        // Assert
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task GivenNameIsNullOrEmpty_WhenCreateAsyncIsCalled_ThenArgumentExceptionIsThrown()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new GameDto { Name = null }));
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new GameDto { Name = string.Empty }));
    }

    [Fact]
    public async Task GivenGameExists_WhenRemoveAsyncIsCalled_ThenReturnsTrue()
    {
        // Arrange
        var testGame = TestData.GetTestGames().First();
        _mockUnitOfWork.Setup(uow => uow.Games.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _service.RemoveAsync(testGame.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GivenGameCannotBeDeleted_WhenRemoveAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        var testGame = TestData.GetTestGames().First();
        _mockUnitOfWork.Setup(uow => uow.Games.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveAsync(testGame.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenRemoveAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync((Game?)null);

        // Act
        var result = await _service.RemoveAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenRemoveAsyncIsCalled_ThenFalseIsReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Games.DeleteByIdAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveAsync(id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenGameExists_WhenRemoveByKeyAsyncIsCalled_ThenReturnsTrue()
    {
        // Arrange
        var testGame = TestData.GetTestGames().First();
        _mockUnitOfWork.Setup(uow => uow.Games.DeleteByKeyAsync(It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var result = await _service.RemoveByKeyAsync(testGame.Key);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GivenGameCannotBeDeleted_WhenRemoveByKeyAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        var testGame = TestData.GetTestGames().First();
        _mockUnitOfWork.Setup(uow => uow.Games.DeleteByKeyAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveByKeyAsync(testGame.Key);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenRemoveByKeyAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync((Game?)null);

        // Act
        var result = await _service.RemoveByKeyAsync(" ");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenValidGameDto_WhenUpdateAsyncIsCalled_ThenReturnsGame()
    {
        // Arrange
        var testGame = TestData.GetTestGamesDTO().First();
        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync(new Game());
        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(new Game());

        // Act
        var result = await _service.UpdateAsync(testGame);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGame.Name, result.Name);
    }

    [Fact]
    public async Task GivenValidGameDto_WhenUpdateAsyncIsCalled_ThenReturnsUpdatedGame()
    {
        // Arrange
        var testGame = TestData.GetTestGamesDTO().First();
        var gameEntity = new Game { Id = testGame.Id, Name = testGame.Name };
        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync(gameEntity);
        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(gameEntity);

        // Act
        var result = await _service.UpdateAsync(testGame);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGame.Name, result.Name);
        _mockUnitOfWork.Verify(uow => uow.Games.UpdateAsync(It.IsAny<Game>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenValidGameDto_WhenUpdateAsyncFailsToComplete_ThenThrowsException()
    {
        // Arrange
        var testGame = TestData.GetTestGamesDTO().First();
        var gameEntity = new Game { Id = testGame.Id, Name = testGame.Name };
        _mockUnitOfWork.Setup(uow => uow.Games.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync(gameEntity);
        _mockMapper.Setup(m => m.Map<Game>(It.IsAny<GameDto>())).Returns(gameEntity);
        _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ThrowsAsync(new Exception("Failed to save changes"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await _service.UpdateAsync(testGame));
    }

    [Fact]
    public async Task GivenNullGameDto_WhenUpdateAsyncIsCalled_ThenThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateAsync(null));
    }

    [Fact]
    public async Task GivenDtoIsNull_WhenUpdateAsyncIsCalled_ThenArgumentNullExceptionIsThrown()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
    }

    [Fact]
    public async Task GivenGameExists_WhenGetGenresAsyncIsCalled_ThenReturnsGenres()
    {
        // Arrange
        var testGenres = TestData.GetTestGenres();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync(TestData.GetTestGame());
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(TestData.GetTestGameDTO());

        // Act
        var result = await _service.GetGenresAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGenres.Count, result.Count());
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenGetGenresAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGenres = TestData.GetTestGenres();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GetGenresAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameExists_WhenGetPlatformsAsyncIsCalled_ThenReturnsPlatforms()
    {
        // Arrange
        var testPlatforms = TestData.GetTestPlatforms();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync(TestData.GetTestGame());
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(TestData.GetTestGameDTO());

        // Act
        var result = await _service.GetPlatformsAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testPlatforms.Count, result.Count());
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenGetPlatformsAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testPlatforms = TestData.GetTestPlatforms();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GetPlatformsAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameExists_WhenGenerateGameFileAsyncIsCalled_ThenReturnsByteArray()
    {
        // Arrange
        var testGame = TestData.GetTestGamesDTO().First();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync(new Game());
        _mockMapper.Setup(m => m.Map<GameDto>(It.IsAny<Game>())).Returns(testGame);

        // Act
        var result = await _service.GenerateGameFileAsync(testGame.Key);

        // Assert
        Assert.NotNull(result);
        var content = Encoding.UTF8.GetString(result);
        var deserializedGame = JsonConvert.DeserializeObject<GameDto>(content);
        Assert.Equal(testGame.Key, deserializedGame.Key);
    }

    [Fact]
    public async Task GivenGameDoesNotExist_WhenGenerateGameFileAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGame = TestData.GetTestGamesDTO().First();

        _mockUnitOfWork.Setup(uow => uow.Games.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GenerateGameFileAsync(testGame.Key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGameWithKeyDoesNotExist_WhenGenerateGameFileAsyncIsCalled_ThenNullIsReturned()
    {
        // Arrange
        var key = "non-existent-key";
        _mockUnitOfWork.Setup(u => u.Games.GetByKeyAsync(key)).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GenerateGameFileAsync(key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGamesExist_WhenGetTotalGamesCountAsyncIsCalled_ThenReturnsCorrectCount()
    {
        // Arrange
        var expectedCount = 10;
        _mockUnitOfWork.Setup(uow => uow.Games.GetCountAsync()).ReturnsAsync(expectedCount);

        // Act
        var result = await _service.GetTotalGamesCountAsync();

        // Assert
        Assert.Equal(expectedCount, result);
        _mockUnitOfWork.Verify(uow => uow.Games.GetCountAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenNoGamesExist_WhenGetTotalGamesCountAsyncIsCalled_ThenReturnsZero()
    {
        // Arrange
        var expectedCount = 0;
        _mockUnitOfWork.Setup(uow => uow.Games.GetCountAsync()).ReturnsAsync(expectedCount);

        // Act
        var result = await _service.GetTotalGamesCountAsync();

        // Assert
        Assert.Equal(expectedCount, result);
        _mockUnitOfWork.Verify(uow => uow.Games.GetCountAsync(), Times.Once);
    }
}