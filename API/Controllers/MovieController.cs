using API.DTOs;
using API.DTOs.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(API.Services.MovieService movieService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await movieService.GetMovieByIdAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [AllowAnonymous]
    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title)
    {
        try
        {
            var result = await movieService.GetMovieByTitleAsync(title);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await movieService.GetAllMoviesAsync();
            return Ok(result);
        }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [AllowAnonymous]
    [HttpGet("genre/{genre}")]
    public async Task<IActionResult> GetByGenre(string genre)
    {
        try
        {
            var result = await movieService.GetMoviesByGenreAsync(genre);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [AllowAnonymous]
    [HttpGet("director/{director}")]
    public async Task<IActionResult> GetByDirector(string director)
    {
        try
        {
            var result = await movieService.GetMoviesByDirectorAsync(director);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [AllowAnonymous]
    [HttpGet("actor/{actor}")]
    public async Task<IActionResult> GetByActor(string actor)
    {
        try
        {
            var result = await movieService.GetMoviesByActorAsync(actor);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovieDto dto)
    {
        try
        {
            var result = await movieService.CreateMovieAsync(
                dto.Title,
                dto.Genres,
                dto.Directors,
                dto.Actors,
                dto.RunTime,
                dto.ReleaseDate,
                dto.Description,
                dto.PosterUrl
            );
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDto dto)
    {
        try
        {
            var result = await movieService.UpdateMovieAsync(
                id,
                dto.Title,
                dto.Genres,
                dto.Directors,
                dto.Actors,
                dto.RunTime,
                dto.ReleaseDate,
                dto.Description,
                dto.PosterUrl
            );
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await movieService.DeleteMovieAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }
}