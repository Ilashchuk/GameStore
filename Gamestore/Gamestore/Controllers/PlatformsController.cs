using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Gamestore.Models;
using Gamestore.Models.RequestViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController : Controller
{
    private readonly IPlatformControlService _platformControlService;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformControlService platformControlService, IMapper mapper)
    {
        _platformControlService = platformControlService;
        _mapper = mapper;
    }

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetPlatforms()
    {
        var platforms = _mapper.Map<List<PlatformViewModel>>(await _platformControlService.GetAllAsync());
        return platforms == null ? BadRequest("Ops! Something went wrong!") : Ok(platforms);
    }

    [HttpGet("find/{id}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetPlatformById(Guid id)
    {
        var platform = _mapper.Map<PlatformViewModel>(await _platformControlService.GetByIdAsync(id));
        return platform == null ? BadRequest("Ops! Something went wrong!") : Ok(platform);
    }

    [HttpPost]
    public async Task<IActionResult> AddPlatform(AddUpdatePlatformViewModel model)
    {
        if (ModelState.IsValid)
        {
            PlatformDto? newPlatformDto = await _platformControlService.CreateAsync(_mapper.Map<PlatformDto>(model));
            return newPlatformDto != null ? CreatedAtAction(nameof(GetPlatformById), new { id = newPlatformDto.Id }, _mapper.Map<PlatformViewModel>(newPlatformDto))
                : BadRequest("Save changes Exeption!");
        }

        return BadRequest("Incorrect data!");
    }

    [HttpPut]
    public async Task<IActionResult> Update(AddUpdatePlatformViewModel model)
    {
        if (ModelState.IsValid)
        {
            PlatformDto? updatePlatformDto = await _platformControlService.UpdateAsync(_mapper.Map<PlatformDto>(model));
            return updatePlatformDto != null ? StatusCode(StatusCodes.Status200OK, model) : BadRequest("Save changes Exeption!");
        }

        return BadRequest("Incorrect data!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _platformControlService.RemoveAsync(id) ? NoContent() : BadRequest("Remove Exeption!");
    }

    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGames(Guid id)
    {
        var games = _mapper.Map<List<GameViewModel>>(await _platformControlService.GetGamesAsync(id));
        return games == null ? BadRequest("Ops! Something went wrong!") : Ok(games);
    }
}
