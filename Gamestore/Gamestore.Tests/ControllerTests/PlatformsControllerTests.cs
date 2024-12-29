using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Gamestore.Controllers;
using Gamestore.Models;
using Gamestore.Models.RequestViewModels;
using Gamestore.Tests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gamestore.Tests.ControllerTests;

public class PlatformsControllerTests : IDisposable
{
    private readonly Mock<IPlatformControlService> _mockPlatformService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PlatformsController _controller;

    public PlatformsControllerTests()
    {
        _mockPlatformService = new Mock<IPlatformControlService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new PlatformsController(_mockPlatformService.Object, _mockMapper.Object);
    }

    public void Dispose()
    {
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetPlatformsShouldReturnOkResultWithPlatforms()
    {
        // Arrange
        var platformsDTO = TestData.GetTestPlatformsDTO();
        var platformsViewModel = TestData.GetTestPlatformsViewModel();

        _mockPlatformService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(platformsDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<PlatformViewModel>>(platformsDTO))
            .Returns(platformsViewModel);

        // Act
        var result = await _controller.GetPlatforms() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<List<PlatformViewModel>>(result.Value);
        Assert.Equal(platformsViewModel.Count, ((List<PlatformViewModel>)result.Value).Count);
    }

    [Fact]
    public async Task GetPlatformByIdShouldReturnOkResultWithPlatform()
    {
        // Arrange
        var platformDTO = TestData.GetTestPlatformDTO();
        var platformViewModel = TestData.GetTestPlatformViewModel();

        _mockPlatformService.Setup(service => service.GetByIdAsync(platformDTO.Id))
            .ReturnsAsync(platformDTO);
        _mockMapper.Setup(mapper => mapper.Map<PlatformViewModel>(platformDTO))
            .Returns(platformViewModel);

        // Act
        var result = await _controller.GetPlatformById(platformDTO.Id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<PlatformViewModel>(result.Value);
        Assert.Equal(platformViewModel.Id, ((PlatformViewModel)result.Value).Id);
    }

    [Fact]
    public async Task AddPlatformShouldReturnCreatedStatusWhenPlatformIsValid()
    {
        // Arrange
        var addPlatformViewModel = TestData.GetTestAddUpdateTestPlatformViewModel();
        var platformDTO = TestData.GetTestPlatformDTO();

        _mockMapper.Setup(mapper => mapper.Map<PlatformDto>(addPlatformViewModel))
            .Returns(platformDTO);
        _mockPlatformService.Setup(service => service.CreateAsync(platformDTO))
            .ReturnsAsync(platformDTO);

        // Act
        var result = await _controller.AddPlatform(addPlatformViewModel) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task UpdateShouldReturnOkStatusWhenPlatformIsUpdated()
    {
        // Arrange
        var platformViewModel = new PlatformViewModel { Id = Guid.NewGuid(), Type = "Updated Platform" };
        var updatePlatformViewModel = new AddUpdatePlatformViewModel { Platform = platformViewModel };
        var platformDTO = new PlatformDto { Id = updatePlatformViewModel.Platform.Id, Type = "Updated Platform" };

        _mockMapper.Setup(mapper => mapper.Map<PlatformDto>(updatePlatformViewModel))
            .Returns(platformDTO);
        _mockPlatformService.Setup(service => service.UpdateAsync(platformDTO))
            .ReturnsAsync(platformDTO);

        // Act
        var result = await _controller.Update(updatePlatformViewModel) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(updatePlatformViewModel, result.Value);
    }

    [Fact]
    public async Task DeleteShouldReturnNoContentWhenDeletionSuccessful()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        _mockPlatformService.Setup(service => service.RemoveAsync(platformId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(platformId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteShouldReturnBadRequestWhenDeletionFails()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        _mockPlatformService.Setup(service => service.RemoveAsync(platformId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(platformId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetGamesShouldReturnOkResultWithGames()
    {
        // Arrange
        var platformId = TestData.GetTestPlatformViewModel().Id;
        var gamesDTO = TestData.GetTestGamesDTO();
        var gamesViewModel = TestData.GetTestGamesViewModel();

        _mockPlatformService.Setup(service => service.GetGamesAsync(platformId))
            .ReturnsAsync(gamesDTO);
        _mockMapper.Setup(mapper => mapper.Map<List<GameViewModel>>(gamesDTO))
            .Returns(gamesViewModel);

        // Act
        var result = await _controller.GetGames(platformId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<List<GameViewModel>>(result.Value);
    }
}
