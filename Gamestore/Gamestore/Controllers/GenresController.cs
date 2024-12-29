using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Gamestore.Models;
using Gamestore.Models.RequestViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController : Controller
{
    private readonly IGenreControlService _genreControlService;
    private readonly IMapper _mapper;

    public GenresController(IGenreControlService genreControlService, IMapper mapper)
    {
        _genreControlService = genreControlService;
        _mapper = mapper;
    }

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGenres()
    {
        var genres = _mapper.Map<List<GenreViewModel>>(await _genreControlService.GetAllAsync());
        return genres == null ? BadRequest("Ops! Something went wrong!") : Ok(genres);
    }

    [HttpGet("find/{id}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var genre = _mapper.Map<GenreViewModel>(await _genreControlService.GetByIdAsync(id));
        return genre == null ? BadRequest("Invalid ID!") : Ok(genre);
    }

    [HttpPost]
    public async Task<IActionResult> AddGenre(AddUpdateGenreViewModel model)
    {
        if (ModelState.IsValid)
        {
            GenreDto? newGenreDto = await _genreControlService.CreateAsync(_mapper.Map<GenreDto>(model));
            return newGenreDto != null ? CreatedAtAction(nameof(GetGenreById), new { id = newGenreDto.Id }, _mapper.Map<GenreViewModel>(newGenreDto))
                : BadRequest("Save changes Exeption!");
        }

        return BadRequest("Incorrect data!");
    }

    [HttpPut]
    public async Task<IActionResult> Update(AddUpdateGenreViewModel model)
    {
        if (ModelState.IsValid && model.Genre.Id != model.Genre.ParentGenreId)
        {
            GenreDto? updateGenreDto = await _genreControlService.UpdateAsync(_mapper.Map<GenreDto>(model));
            return updateGenreDto != null ? StatusCode(StatusCodes.Status200OK, model) : BadRequest("Save changes Exeption!");
        }

        return BadRequest("Incorrect data!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _genreControlService.RemoveAsync(id) ? NoContent() : BadRequest("Remove Exeption!");
    }

    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetGames(Guid id)
    {
        var games = _mapper.Map<List<GameViewModel>>(await _genreControlService.GetGamesAsync(id));
        return games == null ? BadRequest("Ops! Something went wrong!") : Ok(games);
    }

    [HttpGet("{id}/genres")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetSubGenres(Guid id)
    {
        var subGenres = _mapper.Map<List<GenreViewModel>>(await _genreControlService.GetSubGenresAsync(id));
        return subGenres == null ? BadRequest("Ops! Something went wrong!") : Ok(subGenres);
    }
}
