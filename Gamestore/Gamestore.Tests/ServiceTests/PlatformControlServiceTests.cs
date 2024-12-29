using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.UnitOfWork;
using Gamestore.Tests.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gamestore.Tests.ServiceTests;

public class PlatformControlServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PlatformControlService _service;
    private readonly Mock<ILogger<PlatformControlService>> _mockLogger;

    public PlatformControlServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PlatformControlService>>();
        _service = new PlatformControlService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GivenPlatformsExist_WhenGetAllAsyncIsCalled_ThenReturnsList()
    {
        // Arrange
        var testPlatforms = TestData.GetTestPlatforms();
        _mockUnitOfWork.Setup(uow => uow.Platforms.GetAllAsync()).ReturnsAsync(testPlatforms);
        _mockMapper.Setup(m => m.Map<IEnumerable<PlatformDto>>(It.IsAny<IEnumerable<Platform>>()))
                   .Returns(TestData.GetTestPlatformsDTO());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testPlatforms.Count, result.Count());
    }

    [Fact]
    public async Task GivenNoPlatformExist_WhenGetAllAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testPlatforms = TestData.GetTestPlatforms();

        _mockUnitOfWork.Setup(uow => uow.Platforms.GetAllAsync()).ReturnsAsync((List<Platform>?)null);
        _mockMapper.Setup(m => m.Map<IEnumerable<PlatformDto>>(It.IsAny<IEnumerable<Platform>>()))
                   .Returns(TestData.GetTestPlatformsDTO());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenPlatformExists_WhenGetByIdAsyncIsCalled_ThenReturnsPlatform()
    {
        // Arrange
        var testPlatform = TestData.GetTestPlatform();
        var testPlatformDto = TestData.GetTestPlatformDTO();

        _mockUnitOfWork.Setup(uow => uow.Platforms.GetByIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync(testPlatform);
        _mockMapper.Setup(m => m.Map<PlatformDto>(It.IsAny<Platform>())).Returns(testPlatformDto);

        // Act
        var result = await _service.GetByIdAsync(testPlatform.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testPlatform.Id, result.Id);
    }

    [Fact]
    public async Task GivenPlatformDoesNotExist_WhenGetByIdAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testPlatform = TestData.GetTestPlatform();
        var testPlatformDto = TestData.GetTestPlatformDTO();

        _mockUnitOfWork.Setup(uow => uow.Platforms.GetByIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Platform)null);
        _mockMapper.Setup(m => m.Map<PlatformDto>(It.IsAny<Platform>())).Returns(testPlatformDto);

        // Act
        var result = await _service.GetByIdAsync(testPlatform.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenPlatformDoesNotExist_WhenGetByIdAsyncIsCalled_ThenNullIsReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Platforms.GetByIdAsync(id, true)).ReturnsAsync((Platform)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenValidPlatformDto_WhenCreateAsyncIsCalled_ThenReturnsCreatedPlatform()
    {
        // Arrange
        var newPlatformDto = TestData.GetTestPlatformDTO();
        var newPlatform = TestData.GetTestPlatform();

        _mockMapper.Setup(m => m.Map<Platform>(It.IsAny<PlatformDto>())).Returns(newPlatform);
        _mockMapper.Setup(m => m.Map<PlatformDto>(It.IsAny<Platform>())).Returns(newPlatformDto);
        _mockUnitOfWork.Setup(uow => uow.Platforms.AddAsync(It.IsAny<Platform>()));

        // Act
        var result = await _service.CreateAsync(newPlatformDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newPlatformDto.Type, result.Type);
    }

    [Fact]
    public async Task GivenValidPlatformDto_WhenCreateAsyncIsCalled_ThenCallsAddAndComplete()
    {
        // Arrange
        var newPlatformDto = TestData.GetTestPlatformDTO();
        var newPlatform = TestData.GetTestPlatform();

        _mockMapper.Setup(m => m.Map<Platform>(It.IsAny<PlatformDto>())).Returns(newPlatform);
        _mockMapper.Setup(m => m.Map<PlatformDto>(It.IsAny<Platform>())).Returns(newPlatformDto);
        _mockUnitOfWork.Setup(uow => uow.Platforms.AddAsync(It.IsAny<Platform>()));

        // Act
        await _service.CreateAsync(newPlatformDto);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Platforms.AddAsync(It.IsAny<Platform>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenNullPlatformDto_WhenCreateAsyncIsCalled_ThenThrowsArgumentNullException()
    {
        // Arrange
        PlatformDto? newPlatformDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(newPlatformDto!));
    }

    [Fact]
    public async Task GivenValidPlatformDto_WhenCompleteAsyncFails_ThenThrowsException()
    {
        // Arrange
        var newPlatformDto = TestData.GetTestPlatformDTO();
        var newPlatform = TestData.GetTestPlatform();

        _mockMapper.Setup(m => m.Map<Platform>(It.IsAny<PlatformDto>())).Returns(newPlatform);
        _mockUnitOfWork.Setup(uow => uow.Platforms.AddAsync(It.IsAny<Platform>()));
        _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(newPlatformDto));
        Assert.Equal("Database error", exception.Message);
    }

    [Fact]
    public async Task GivenPlatformDtoWithEmptyType_WhenCreateAsyncIsCalled_ThenThrowsArgumentException()
    {
        // Arrange
        var newPlatformDto = TestData.GetTestPlatformDTO();
        newPlatformDto.Type = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(newPlatformDto));
    }

    [Fact]
    public async Task GivenMapperFailsToMap_WhenCreateAsyncIsCalled_ThenThrowsException()
    {
        // Arrange
        var newPlatformDto = TestData.GetTestPlatformDTO();

        _mockMapper.Setup(m => m.Map<Platform>(It.IsAny<PlatformDto>()))
                   .Throws(new InvalidOperationException("Mapping failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(newPlatformDto));
        Assert.Equal("Mapping failed", exception.Message);
    }

    [Fact]
    public async Task GivenExceptionOccursInAddAsync_WhenCreateAsyncIsCalled_ThenDoesNotCallCompleteAsync()
    {
        // Arrange
        var newPlatformDto = TestData.GetTestPlatformDTO();

        _mockUnitOfWork.Setup(uow => uow.Platforms.AddAsync(It.IsAny<Platform>()))
                       .ThrowsAsync(new Exception("AddAsync failed"));

        // Act
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(newPlatformDto));

        // Assert
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task GivenTypeIsNullOrEmpty_WhenCreateAsyncIsCalled_ThenArgumentExceptionIsThrown()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new PlatformDto { Type = null }));
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new PlatformDto { Type = string.Empty }));
    }

    [Fact]
    public async Task GivenPlatformExists_WhenRemoveAsyncIsCalled_ThenReturnsTrue()
    {
        // Arrange
        var testPlatform = TestData.GetTestPlatform();
        _mockUnitOfWork.Setup(uow => uow.Platforms.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _service.RemoveAsync(testPlatform.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GivenPlatformCannotBeDeleted_WhenRemoveAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        var testPlatform = TestData.GetTestPlatform();
        _mockUnitOfWork.Setup(uow => uow.Platforms.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveAsync(testPlatform.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenPlatformDoesNotExist_WhenRemoveAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.Platforms.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync((Platform?)null);

        // Act
        var result = await _service.RemoveAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenPlatformDoesNotExist_WhenRemoveAsyncIsCalled_ThenFalseIsReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Platforms.DeleteByIdAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveAsync(id);

        // Assert
        Assert.False(result);
    }
}
