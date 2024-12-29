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

public class GenresControllerTests : IDisposable
{
    private readonly Mock<IGenreControlService> _mockGenreControlService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GenresController _controller;

    public GenresControllerTests()
    {
        _mockGenreControlService = new Mock<IGenreControlService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new GenresController(_mockGenreControlService.Object, _mockMapper.Object);
    }

    public void Dispose()
    {
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetGenresShouldReturnOkWithGenres()
    {
        // Arrange
        var genresDTO = TestData.GetTestGenresDTO();
        var genresViewModel = TestData.GetTestGenresViewModel();

        _mockGenreControlService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(genresDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<GenreViewModel>>(genresDTO))
            .Returns(genresViewModel);

        // Act
        var result = await _controller.GetGenres() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(genresViewModel, result.Value);
    }

    [Fact]
    public async Task GetGenreByIdShouldReturnOkWithGenreWhenGenreExists()
    {
        // Arrange
        var genreId = TestData.GetTestGenreViewModel().Id;
        var genreDTO = TestData.GetTestGenreDTO();
        var genreViewModel = TestData.GetTestGenreViewModel();

        _mockGenreControlService.Setup(service => service.GetByIdAsync(genreId))
            .ReturnsAsync(genreDTO);
        _mockMapper.Setup(mapper => mapper.Map<GenreViewModel>(genreDTO))
            .Returns(genreViewModel);

        // Act
        var result = await _controller.GetGenreById(genreId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(genreViewModel, result.Value);
    }

    [Fact]
    public async Task AddGenreShouldReturnCreatedWhenModelIsValid()
    {
        // Arrange
        var genreViewModel = TestData.GetTestAddUpdateTestGenreViewModel();
        var genreDTO = TestData.GetTestGenreDTO();

        _mockMapper.Setup(mapper => mapper.Map<GenreDto>(genreViewModel))
            .Returns(genreDTO);
        _mockGenreControlService.Setup(service => service.CreateAsync(genreDTO))
            .ReturnsAsync(genreDTO);

        // Act
        var result = await _controller.AddGenre(genreViewModel) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task UpdateShouldReturnOkWhenModelIsValid()
    {
        // Arrange
        var genreViewModel = TestData.GetTestAddUpdateTestGenreViewModel();
        var genreDTO = TestData.GetTestGenreDTO();

        _mockMapper.Setup(mapper => mapper.Map<GenreDto>(genreViewModel))
            .Returns(genreDTO);
        _mockGenreControlService.Setup(service => service.UpdateAsync(genreDTO))
            .ReturnsAsync(genreDTO);

        // Act
        var result = await _controller.Update(genreViewModel) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(genreViewModel, result.Value);
    }

    [Fact]
    public async Task DeleteShouldReturnNoContentWhenDeletionSucceeds()
    {
        // Arrange
        var genreId = TestData.GetTestGenreViewModel().Id;

        _mockGenreControlService.Setup(service => service.RemoveAsync(genreId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(genreId) as NoContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [Fact]
    public async Task GetGamesShouldReturnOkWithGames()
    {
        // Arrange
        var genreId = TestData.GetTestGenreViewModel().Id;
        var gamesDTO = TestData.GetTestGamesDTO();
        var gamesViewModel = TestData.GetTestGamesViewModel();

        _mockGenreControlService.Setup(service => service.GetGamesAsync(genreId))
            .ReturnsAsync(gamesDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<GameViewModel>>(gamesDTO))
            .Returns(gamesViewModel);

        // Act
        var result = await _controller.GetGames(genreId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(gamesViewModel, result.Value);
    }

    [Fact]
    public async Task GetSubGenresShouldReturnOkWithSubGenres()
    {
        // Arrange
        var genreId = TestData.GetTestGenreViewModel().Id;
        var subGenresDTO = TestData.GetTestGenresDTO();
        var subGenresViewModel = TestData.GetTestGenresViewModel();

        _mockGenreControlService.Setup(service => service.GetSubGenresAsync(genreId))
            .ReturnsAsync(subGenresDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<GenreViewModel>>(subGenresDTO))
            .Returns(subGenresViewModel);

        // Act
        var result = await _controller.GetSubGenres(genreId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(subGenresViewModel, result.Value);
    }
}
