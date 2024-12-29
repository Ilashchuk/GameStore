using System.Text;
using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BLL.Services;

public class GameControlService : IGameControlService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GameControlService> _logger;

    public GameControlService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GameControlService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<GameDto>?> GetAllAsync()
    {
        _logger.LogInformation("Executing {MethodName}", nameof(GetAllAsync));
        var games = await _unitOfWork.Games.GetAllAsync();

        if (games == null)
        {
            _logger.LogWarning("Games not found");
            return null;
        }

        _logger.LogInformation("Retrieved {GameCount} games from the database", games.Count);
        return _mapper.Map<IEnumerable<GameDto>>(games);
    }

    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} with ID: {Id}", nameof(GetByIdAsync), id);
        var game = await _unitOfWork.Games.GetByIdAsync(id, true);

        if (game == null)
        {
            _logger.LogWarning("Game with ID {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Game with ID {Id} retrieved successfully", id);
        return _mapper.Map<GameDto>(game);
    }

    public async Task<GameDto?> GetByKeyAsync(string key)
    {
        _logger.LogInformation("Executing {MethodName} with Key: {Key}", nameof(GetByKeyAsync), key);
        var game = await _unitOfWork.Games.GetByKeyAsync(key);

        if (game == null)
        {
            _logger.LogWarning("Game with Key {Key} not found", key);
            return null;
        }

        _logger.LogInformation("Game with Key {Key} retrieved successfully", key);
        return _mapper.Map<GameDto>(game);
    }

    public async Task<GameDto?> CreateAsync(GameDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new ArgumentException(nameof(dto));
        }

        _logger.LogInformation("Executing {MethodName} with Game Name: {Name}", nameof(CreateAsync), dto.Name);

        if (string.IsNullOrEmpty(dto.Key))
        {
            dto.Key = dto.Name.ToLower(System.Globalization.CultureInfo.CurrentCulture) + Guid.NewGuid();
            _logger.LogInformation("Generated Key: {Key} for Game Name: {Name}", dto.Key, dto.Name);
        }

        var gameEntity = _mapper.Map<Game>(dto);

        await _unitOfWork.Games.AddAsync(gameEntity);
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Game with Name {Name} created successfully with ID {Id}", dto.Name, gameEntity?.Id);

        return _mapper.Map<GameDto>(gameEntity);
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} for Game ID: {Id}", nameof(RemoveAsync), id);

        if (await _unitOfWork.Games.DeleteByIdAsync(id))
        {
            _logger.LogInformation("Game with ID {Id} removed successfully", id);
            return true;
        }

        _logger.LogWarning("Game with ID {Id} could not be removed", id);
        return false;
    }

    public async Task<bool> RemoveByKeyAsync(string key)
    {
        _logger.LogInformation("Executing {MethodName} for Game Key: {Key}", nameof(RemoveByKeyAsync), key);

        if (await _unitOfWork.Games.DeleteByKeyAsync(key))
        {
            _logger.LogInformation("Game with Key {Key} removed successfully", key);
            return true;
        }

        _logger.LogWarning("Game with Key {Key} could not be removed", key);
        return false;
    }

    public async Task<GameDto?> UpdateAsync(GameDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        _logger.LogInformation("Executing {MethodName} for Game ID: {Id}", nameof(UpdateAsync), dto.Id);

        await _unitOfWork.Games.UpdateAsync(_mapper.Map<Game>(dto));
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Game with ID {Id} updated successfully", dto.Id);
        return dto;
    }

    public async Task<IEnumerable<GenreDto?>> GetGenresAsync(string key)
    {
        _logger.LogInformation("Executing {MethodName} for Game Key: {Key}", nameof(GetGenresAsync), key);
        var game = await GetByKeyAsync(key);

        if (game == null)
        {
            _logger.LogWarning("Game with Key {Key} not found", key);
            return null;
        }

        _logger.LogInformation("Retrieved {GenreCount} genres for Game Key {Key}", game.GenresDto.Count, key);
        return game.GenresDto;
    }

    public async Task<IEnumerable<PlatformDto?>> GetPlatformsAsync(string key)
    {
        _logger.LogInformation("Executing {MethodName} for Game Key: {Key}", nameof(GetPlatformsAsync), key);
        var game = await GetByKeyAsync(key);

        if (game == null)
        {
            _logger.LogWarning("Game with Key {Key} not found", key);
            return null;
        }

        _logger.LogInformation("Retrieved {PlatformCount} platforms for Game Key {Key}", game.PlatformsDto.Count, key);
        return game.PlatformsDto;
    }

    public async Task<byte[]> GenerateGameFileAsync(string key)
    {
        _logger.LogInformation("Executing {MethodName} for Game Key: {Key}", nameof(GenerateGameFileAsync), key);
        var game = await GetByKeyAsync(key);

        if (game == null)
        {
            _logger.LogWarning("Game with Key {Key} not found for file generation", key);
            return null;
        }

        string content = JsonConvert.SerializeObject(game, Formatting.Indented);

        if (string.IsNullOrEmpty(content))
        {
            _logger.LogWarning("Generated content for Game Key {Key} is empty", key);
            return null;
        }

        var bytes = Encoding.UTF8.GetBytes(content);
        _logger.LogInformation("File for Game Key {Key} generated successfully", key);
        return bytes;
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        _logger.LogInformation("Executing {MethodName}", nameof(GetTotalGamesCountAsync));
        var count = await _unitOfWork.Games.GetCountAsync();
        _logger.LogInformation("Total number of games: {Count}", count);
        return count;
    }
}
