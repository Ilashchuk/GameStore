using System.Text;
using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Gamestore.Controllers;
using Gamestore.Models;
using Gamestore.Tests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gamestore.Tests.ControllerTests;

public class GamesControllerTests : IDisposable
{
    private readonly Mock<IGameControlService> _mockGameControlService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _mockGameControlService = new Mock<IGameControlService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new GamesController(_mockGameControlService.Object, _mockMapper.Object);
    }

    public void Dispose()
    {
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetGamesShouldReturnOkWithGames()
    {
        // Arrange
        var gamesDTO = TestData.GetTestGamesDTO();
        var gamesViewModel = TestData.GetTestGamesViewModel();

        _mockGameControlService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(gamesDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<GameViewModel>>(gamesDTO))
            .Returns(gamesViewModel);

        // Act
        var result = await _controller.GetGames() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(gamesViewModel, result.Value);
    }

    [Fact]
    public async Task GetGameByIdShouldReturnOkWithGameWhenGameExists()
    {
        // Arrange
        var gameId = TestData.GetTestGameViewModel().Id;
        var gameDTO = TestData.GetTestGameDTO();
        var gameViewModel = TestData.GetTestGameViewModel();

        _mockGameControlService.Setup(service => service.GetByIdAsync(gameId))
            .ReturnsAsync(gameDTO);
        _mockMapper.Setup(mapper => mapper.Map<GameViewModel>(gameDTO))
            .Returns(gameViewModel);

        // Act
        var result = await _controller.GetGameById(gameId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(gameViewModel, result.Value);
    }

    [Fact]
    public async Task GetGameByKeyShouldReturnOkWithGameWhenGameExists()
    {
        // Arrange
        var gameKey = TestData.GetTestGameViewModel().Key;
        var gameDTO = TestData.GetTestGameDTO();
        var gameViewModel = TestData.GetTestGameViewModel();

        _mockGameControlService.Setup(service => service.GetByKeyAsync(gameKey))
            .ReturnsAsync(gameDTO);
        _mockMapper.Setup(mapper => mapper.Map<GameViewModel>(gameDTO))
            .Returns(gameViewModel);

        // Act
        var result = await _controller.GetGameByKey(gameKey) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(gameViewModel, result.Value);
    }

    [Fact]
    public async Task AddGameShouldReturnCreatedWhenModelIsValid()
    {
        // Arrange
        var gameViewModel = TestData.GetTestAddUpdateTestGameViewModel();
        var gameDTO = TestData.GetTestGameDTO();

        _mockMapper.Setup(mapper => mapper.Map<GameDto>(gameViewModel))
            .Returns(gameDTO);
        _mockGameControlService.Setup(service => service.CreateAsync(gameDTO))
            .ReturnsAsync(gameDTO);

        // Act
        var result = await _controller.AddGame(gameViewModel) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task UpdateShouldReturnOkWhenModelIsValid()
    {
        // Arrange
        var gameViewModel = TestData.GetTestAddUpdateTestGameViewModel();
        var gameDTO = TestData.GetTestGameDTO();

        _mockMapper.Setup(mapper => mapper.Map<GameDto>(gameViewModel))
            .Returns(gameDTO);
        _mockGameControlService.Setup(service => service.UpdateAsync(gameDTO))
            .ReturnsAsync(gameDTO);

        // Act
        var result = await _controller.Update(gameViewModel) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(gameViewModel, result.Value);
    }

    [Fact]
    public async Task DeleteShouldReturnNoContentWhenDeletionSucceeds()
    {
        // Arrange
        var gameKey = TestData.GetTestGameViewModel().Key;

        _mockGameControlService.Setup(service => service.RemoveByKeyAsync(gameKey))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(gameKey) as NoContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [Fact]
    public async Task GetGenresShouldReturnOkWithGenres()
    {
        // Arrange
        var gameKey = TestData.GetTestGameViewModel().Key;
        var genresDTO = TestData.GetTestGenresDTO();
        var genresViewModel = TestData.GetTestGenresViewModel();

        _mockGameControlService.Setup(service => service.GetGenresAsync(gameKey))
            .ReturnsAsync(genresDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<GenreViewModel>>(genresDTO))
            .Returns(genresViewModel);

        // Act
        var result = await _controller.GetGenres(gameKey) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(genresViewModel, result.Value);
    }

    [Fact]
    public async Task GetPlatformsShouldReturnOkWithPlatforms()
    {
        // Arrange
        var gameKey = TestData.GetTestGameViewModel().Key;
        var platformsDTO = TestData.GetTestPlatformsDTO();
        var platformsViewModel = TestData.GetTestPlatformsViewModel();

        _mockGameControlService.Setup(service => service.GetPlatformsAsync(gameKey))
            .ReturnsAsync(platformsDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<PlatformViewModel>>(platformsDTO))
            .Returns(platformsViewModel);

        // Act
        var result = await _controller.GetPlatforms(gameKey) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(platformsViewModel, result.Value);
    }

    [Fact]
    public async Task DownloadGameFileShouldReturnFileWhenFileExists()
    {
        // Arrange
        var gameKey = TestData.GetTestGameViewModel().Key;
        var fileContent = Encoding.UTF8.GetBytes("Test file content");
        var expectedFileName = $"{gameKey}_{DateTime.Now:yyyyMMdd}.txt";

        _mockGameControlService.Setup(service => service.GenerateGameFileAsync(gameKey))
            .ReturnsAsync(fileContent);

        // Act
        var result = await _controller.DownloadGameFile(gameKey) as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("text/plain", result.ContentType);
        Assert.Equal(expectedFileName, result.FileDownloadName);
        Assert.Equal(fileContent, result.FileContents);
    }
}
