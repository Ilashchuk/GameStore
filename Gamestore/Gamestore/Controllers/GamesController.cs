using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Gamestore.Models;
using Gamestore.Models.RequestViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Controllers;

[Route("[controller]")]
[ApiController]
public class GamesController : Controller
{
    private readonly IGameControlService _gameControlService;
    private readonly IMapper _mapper;

    public GamesController(IGameControlService gameControlService, IMapper mapper)
    {
        _gameControlService = gameControlService;
        _mapper = mapper;
    }

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGames()
    {
        var games = _mapper.Map<List<GameViewModel>>(await _gameControlService.GetAllAsync());
        return games == null ? BadRequest("Ops! Something went wrong!") : Ok(games);
    }

    [HttpGet("find/{id}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        var game = _mapper.Map<GameViewModel>(await _gameControlService.GetByIdAsync(id));
        return game == null ? BadRequest("Invalid ID!") : Ok(game);
    }

    [HttpGet("{key}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGameByKey(string key)
    {
        var game = _mapper.Map<GameViewModel>(await _gameControlService.GetByKeyAsync(key));
        return game == null ? BadRequest("Invalid KEY!") : Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> AddGame(AddUpdateGameViewModel model)
    {
        if (ModelState.IsValid)
        {
            GameDto? newGameDto = await _gameControlService.CreateAsync(_mapper.Map<GameDto>(model));
            return newGameDto != null ? CreatedAtAction(nameof(GetGameById), new { id = newGameDto.Id }, _mapper.Map<GameViewModel>(newGameDto))
                : BadRequest("Save changes Exeption!");
        }

        return BadRequest("Incorrect data!");
    }

    [HttpPut]
    public async Task<IActionResult> Update(AddUpdateGameViewModel model)
    {
        if (ModelState.IsValid)
        {
            GameDto? updateGameDto = await _gameControlService.UpdateAsync(_mapper.Map<GameDto>(model));
            return updateGameDto != null ? StatusCode(StatusCodes.Status200OK, model) : BadRequest("Save changes Exeption!");
        }

        return BadRequest("Incorrect data!");
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        return await _gameControlService.RemoveByKeyAsync(key) ? NoContent() : BadRequest("Remove Exeption!");
    }

    [HttpGet("/{key}/genres")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGenres(string key)
    {
        var genres = _mapper.Map<List<GenreViewModel>>(await _gameControlService.GetGenresAsync(key));
        return genres == null ? BadRequest("Ops! Something went wrong!") : Ok(genres);
    }

    [HttpGet("/{key}/platforms")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetPlatforms(string key)
    {
        var platforms = _mapper.Map<List<PlatformViewModel>>(await _gameControlService.GetPlatformsAsync(key));
        return platforms == null ? BadRequest("Ops! Something went wrong!") : Ok(platforms);
    }

    [HttpGet("{key}/file")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> DownloadGameFile(string key)
    {
        var fileContent = await _gameControlService.GenerateGameFileAsync(key);
        if (fileContent == null)
        {
            return NotFound();
        }

        var fileName = $"{key}_{DateTime.Now:yyyyMMdd}.txt";
        return File(fileContent, "text/plain", fileDownloadName: fileName);
    }
}
