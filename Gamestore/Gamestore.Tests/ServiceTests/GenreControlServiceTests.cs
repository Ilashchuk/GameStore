using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.UnitOfWork;
using Gamestore.Tests.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gamestore.Tests.ServiceTests;

public class GenreControlServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GenreControlService _service;
    private readonly Mock<ILogger<GenreControlService>> _mockLogger;

    public GenreControlServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GenreControlService>>();
        _service = new GenreControlService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GivenGenresExist_WhenGetAllAsyncIsCalled_ThenReturnsList()
    {
        // Arrange
        var testGenres = TestData.GetTestGenres();
        _mockUnitOfWork.Setup(uow => uow.Genres.GetAllAsync()).ReturnsAsync(testGenres);
        _mockMapper.Setup(m => m.Map<IEnumerable<GenreDto>>(It.IsAny<IEnumerable<Genre>>()))
                   .Returns(TestData.GetTestGenresDTO());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGenres.Count, result.Count());
    }

    [Fact]
    public async Task GivenNoGenresExist_WhenGetAllAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGames = TestData.GetTestGenres();

        _mockUnitOfWork.Setup(uow => uow.Genres.GetAllAsync()).ReturnsAsync((List<Genre>?)null);
        _mockMapper.Setup(m => m.Map<IEnumerable<GenreDto>>(It.IsAny<IEnumerable<Genre>>()))
                   .Returns(TestData.GetTestGenresDTO());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGenreExists_WhenGetByIdAsyncIsCalled_ThenReturnsGenre()
    {
        // Arrange
        var testGenre = TestData.GetTestGenre();
        var testGenreDto = TestData.GetTestGenreDTO();

        _mockUnitOfWork.Setup(uow => uow.Genres.GetByIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync(testGenre);
        _mockMapper.Setup(m => m.Map<GenreDto>(It.IsAny<Genre>())).Returns(testGenreDto);

        // Act
        var result = await _service.GetByIdAsync(testGenre.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testGenre.Id, result.Id);
    }

    [Fact]
    public async Task GivenGenreDoesNotExist_WhenGetByIdAsyncIsCalled_ThenReturnsNull()
    {
        // Arrange
        var testGenre = TestData.GetTestGenre();
        var testGenreDto = TestData.GetTestGenreDTO();

        _mockUnitOfWork.Setup(uow => uow.Genres.GetByIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Genre)null);
        _mockMapper.Setup(m => m.Map<GenreDto>(It.IsAny<Genre>())).Returns(testGenreDto);

        // Act
        var result = await _service.GetByIdAsync(testGenre.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenGenreDoesNotExist_WhenGetByIdAsyncIsCalled_ThenNullIsReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Genres.GetByIdAsync(id, true)).ReturnsAsync((Genre)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GivenValidGenreDto_WhenCreateAsyncIsCalled_ThenReturnsCreatedGenre()
    {
        // Arrange
        var newGenreDto = TestData.GetTestGenreDTO();
        var newGenre = TestData.GetTestGenre();

        _mockMapper.Setup(m => m.Map<Genre>(It.IsAny<GenreDto>())).Returns(newGenre);
        _mockMapper.Setup(m => m.Map<GenreDto>(It.IsAny<Genre>())).Returns(newGenreDto);
        _mockUnitOfWork.Setup(uow => uow.Genres.AddAsync(It.IsAny<Genre>()));

        // Act
        var result = await _service.CreateAsync(newGenreDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newGenreDto.Name, result.Name);
    }

    [Fact]
    public async Task GivenValidGenreDto_WhenCreateAsyncIsCalled_ThenCallsAddAndComplete()
    {
        // Arrange
        var newGenreDto = TestData.GetTestGenreDTO();
        var newGenre = TestData.GetTestGenre();

        _mockMapper.Setup(m => m.Map<Genre>(It.IsAny<GenreDto>())).Returns(newGenre);
        _mockMapper.Setup(m => m.Map<GenreDto>(It.IsAny<Genre>())).Returns(newGenreDto);
        _mockUnitOfWork.Setup(uow => uow.Genres.AddAsync(It.IsAny<Genre>()));

        // Act
        await _service.CreateAsync(newGenreDto);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Genres.AddAsync(It.IsAny<Genre>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenNullGenreDto_WhenCreateAsyncIsCalled_ThenThrowsArgumentNullException()
    {
        // Arrange
        GenreDto? newGenreDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(newGenreDto!));
    }

    [Fact]
    public async Task GivenValidGenreDto_WhenCompleteAsyncFails_ThenThrowsException()
    {
        // Arrange
        var newGenreDto = TestData.GetTestGenreDTO();
        var newGenre = TestData.GetTestGenre();

        _mockMapper.Setup(m => m.Map<Genre>(It.IsAny<GenreDto>())).Returns(newGenre);
        _mockUnitOfWork.Setup(uow => uow.Genres.AddAsync(It.IsAny<Genre>()));
        _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(newGenreDto));
        Assert.Equal("Database error", exception.Message);
    }

    [Fact]
    public async Task GivenGenreDtoWithEmptyName_WhenCreateAsyncIsCalled_ThenThrowsArgumentException()
    {
        // Arrange
        var newGenreDto = TestData.GetTestGenreDTO();
        newGenreDto.Name = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(newGenreDto));
    }

    [Fact]
    public async Task GivenMapperFailsToMap_WhenCreateAsyncIsCalled_ThenThrowsException()
    {
        // Arrange
        var newGenreDto = TestData.GetTestGenreDTO();

        _mockMapper.Setup(m => m.Map<Genre>(It.IsAny<GenreDto>()))
                   .Throws(new InvalidOperationException("Mapping failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(newGenreDto));
        Assert.Equal("Mapping failed", exception.Message);
    }

    [Fact]
    public async Task GivenExceptionOccursInAddAsync_WhenCreateAsyncIsCalled_ThenDoesNotCallCompleteAsync()
    {
        // Arrange
        var newGenreDto = TestData.GetTestGenreDTO();

        _mockUnitOfWork.Setup(uow => uow.Genres.AddAsync(It.IsAny<Genre>()))
                       .ThrowsAsync(new Exception("AddAsync failed"));

        // Act
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(newGenreDto));

        // Assert
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
    }

    [Fact]
    public async Task GivenNameIsNullOrEmpty_WhenCreateAsyncIsCalled_ThenArgumentExceptionIsThrown()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new GenreDto { Name = null }));
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new GenreDto { Name = string.Empty }));
    }

    [Fact]
    public async Task GivenGenreExists_WhenRemoveAsyncIsCalled_ThenReturnsTrue()
    {
        // Arrange
        var testGenre = TestData.GetTestGenre();
        _mockUnitOfWork.Setup(uow => uow.Genres.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _service.RemoveAsync(testGenre.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GivenGenreCannotBeDeleted_WhenRemoveAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        var testGenre = TestData.GetTestGenre();
        _mockUnitOfWork.Setup(uow => uow.Genres.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveAsync(testGenre.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenGenreDoesNotExist_WhenRemoveAsyncIsCalled_ThenReturnsFalse()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.Genres.GetByIdAsync(It.IsAny<Guid>(), false)).ReturnsAsync((Genre?)null);

        // Act
        var result = await _service.RemoveAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenGenreDoesNotExist_WhenRemoveAsyncIsCalled_ThenFalseIsReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Genres.DeleteByIdAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _service.RemoveAsync(id);

        // Assert
        Assert.False(result);
    }
}
