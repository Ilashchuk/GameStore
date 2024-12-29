using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace BLL.Services;

public class GenreControlService : IGenreControlService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GenreControlService> _logger;

    public GenreControlService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GenreControlService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<GenreDto>?> GetAllAsync()
    {
        _logger.LogInformation("Executing {MethodName}", nameof(GetAllAsync));
        var genres = await _unitOfWork.Genres.GetAllAsync();

        if (genres == null)
        {
            _logger.LogWarning("Genres not found");
            return null;
        }

        _logger.LogInformation("Retrieved {GenreCount} genres from the database", genres.Count);
        return _mapper.Map<IEnumerable<GenreDto>>(genres);
    }

    public async Task<GenreDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} with ID: {Id}", nameof(GetByIdAsync), id);
        var genre = await _unitOfWork.Genres.GetByIdAsync(id, true);

        if (genre == null)
        {
            _logger.LogWarning("Genre with ID {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Genre with ID {Id} retrieved successfully", id);
        return _mapper.Map<GenreDto>(genre);
    }

    public async Task<GenreDto?> CreateAsync(GenreDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentException(nameof(dto));
        }

        _logger.LogInformation("Executing {MethodName} with Genre Name: {Name}", nameof(CreateAsync), dto.Name);

        var genreEntity = _mapper.Map<Genre>(dto);

        await _unitOfWork.Genres.AddAsync(genreEntity);
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Genre with Name {Name} created successfully", dto.Name);
        return _mapper.Map<GenreDto>(genreEntity);
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} for Genre ID: {Id}", nameof(RemoveAsync), id);

        if (await _unitOfWork.Genres.DeleteByIdAsync(id))
        {
            _logger.LogInformation("Genre with ID {Id} removed successfully", id);
            return true;
        }

        _logger.LogWarning("Genre with ID {Id} could not be removed", id);
        return false;
    }

    public async Task<GenreDto?> UpdateAsync(GenreDto dto)
    {
        _logger.LogInformation("Executing {MethodName} for Genre ID: {Id}", nameof(UpdateAsync), dto.Id);

        _unitOfWork.Genres.Update(_mapper.Map<Genre>(dto));
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Genre with ID {Id} updated successfully", dto.Id);
        return dto;
    }

    public async Task<IEnumerable<GameDto>?> GetGamesAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} for Genre ID: {Id}", nameof(GetGamesAsync), id);
        var genre = await GetByIdAsync(id);

        if (genre == null)
        {
            _logger.LogWarning("Genre with Id {id} not found", id);
            return null;
        }

        _logger.LogInformation("Retrieved {GameCount} games for Genre ID {Id}", genre.GamesDto.Count, id);
        return genre.GamesDto;
    }

    public async Task<IEnumerable<GenreDto?>> GetSubGenresAsync(Guid parentId)
    {
        _logger.LogInformation("Executing {MethodName} for Parent Genre ID: {ParentId}", nameof(GetSubGenresAsync), parentId);
        var subGenres = await _unitOfWork.Genres.GetSubGenresAsync(parentId);

        if (subGenres == null)
        {
            _logger.LogWarning("Genre with Id {parentId} not found", parentId);
            return null;
        }

        _logger.LogInformation("Retrieved {SubGenreCount} sub-genres for Parent Genre ID {ParentId}", subGenres.Count, parentId);
        return _mapper.Map<IEnumerable<GenreDto>>(subGenres);
    }
}
