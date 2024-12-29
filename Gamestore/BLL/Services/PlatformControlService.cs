using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace BLL.Services;

public class PlatformControlService : IPlatformControlService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PlatformControlService> _logger;

    public PlatformControlService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlatformControlService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PlatformDto>?> GetAllAsync()
    {
        _logger.LogInformation("Executing {MethodName}", nameof(GetAllAsync));
        var platforms = await _unitOfWork.Platforms.GetAllAsync();

        if (platforms == null)
        {
            _logger.LogWarning("Platforms not found");
            return null;
        }

        _logger.LogInformation("Retrieved {PlatformCount} platforms from the database", platforms.Count);
        return _mapper.Map<IEnumerable<PlatformDto>>(platforms);
    }

    public async Task<PlatformDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} with ID: {Id}", nameof(GetByIdAsync), id);
        var platform = await _unitOfWork.Platforms.GetByIdAsync(id, true);

        if (platform == null)
        {
            _logger.LogWarning("Platform with ID {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Platform with ID {Id} retrieved successfully", id);
        return _mapper.Map<PlatformDto>(platform);
    }

    public async Task<PlatformDto?> CreateAsync(PlatformDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (string.IsNullOrEmpty(dto.Type))
        {
            throw new ArgumentException(nameof(dto));
        }

        _logger.LogInformation("Executing {MethodName} with Platform Name: {Name}", nameof(CreateAsync), dto.Type);

        var platformEntity = _mapper.Map<Platform>(dto);

        await _unitOfWork.Platforms.AddAsync(platformEntity);
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Platform with Name {Name} created successfully", dto.Type);
        return _mapper.Map<PlatformDto>(platformEntity);
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} for Platform ID: {Id}", nameof(RemoveAsync), id);

        if (await _unitOfWork.Platforms.DeleteByIdAsync(id))
        {
            _logger.LogInformation("Platform with ID {Id} removed successfully", id);
            return true;
        }

        _logger.LogWarning("Platform with ID {Id} could not be removed", id);
        return false;
    }

    public async Task<PlatformDto?> UpdateAsync(PlatformDto dto)
    {
        _logger.LogInformation("Executing {MethodName} for Platform ID: {Id}", nameof(UpdateAsync), dto.Id);

        _unitOfWork.Platforms.Update(_mapper.Map<Platform>(dto));
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Platform with ID {Id} updated successfully", dto.Id);
        return dto;
    }

    public async Task<IEnumerable<GameDto>?> GetGamesAsync(Guid id)
    {
        _logger.LogInformation("Executing {MethodName} for Platform ID: {Id}", nameof(GetGamesAsync), id);
        var platform = await GetByIdAsync(id);

        if (platform == null)
        {
            _logger.LogWarning("Platform with Id {id} not found", id);
            return null;
        }

        _logger.LogInformation("Retrieved {GameCount} games for Platform ID {Id}", platform.GamesDto.Count, id);
        return platform.GamesDto;
    }
}
