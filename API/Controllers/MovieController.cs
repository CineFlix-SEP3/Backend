using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(API.Services.MovieService movieService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        try
        {
            var result = await movieService.CreateMovieAsync(
                request.Title,
                request.Genres,
                request.Directors,
                request.Actors,
                request.RunTime,
                request.ReleaseDate,
                request.Rating,
                request.Description,
                request.PosterUrl
            );
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await movieService.GetMovieByIdAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title)
    {
        try
        {
            var result = await movieService.GetMovieByTitleAsync(title);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await movieService.GetAllMoviesAsync();
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieRequest request)
    {
        try
        {
            var result = await movieService.UpdateMovieAsync(
                id,
                request.Title,
                request.Genres,
                request.Directors,
                request.Actors,
                request.RunTime,
                request.ReleaseDate,
                request.Rating,
                request.Description,
                request.PosterUrl
            );
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await movieService.DeleteMovieAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("genre/{genre}")]
    public async Task<IActionResult> GetByGenre(string genre)
    {
        try
        {
            var result = await movieService.GetMoviesByGenreAsync(genre);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("director/{director}")]
    public async Task<IActionResult> GetByDirector(string director)
    {
        try
        {
            var result = await movieService.GetMoviesByDirectorAsync(director);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("actor/{actor}")]
    public async Task<IActionResult> GetByActor(string actor)
    {
        try
        {
            var result = await movieService.GetMoviesByActorAsync(actor);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}