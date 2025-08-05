using API.DTOs.Review;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(API.Services.ReviewService reviewService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("movie/{movieId:int}")]
    public async Task<IActionResult> GetByMovie(int movieId)
    {
        try
        {
            var result = await reviewService.GetReviewsByMovieAsync(movieId);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [AllowAnonymous]
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        try
        {
            var result = await reviewService.GetReviewsByUserAsync(userId);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        try
        {
            var result = await reviewService.CreateReviewAsync(
                dto.MovieId, dto.UserId, dto.Text, dto.Rating);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto dto)
    {
        try
        {
            var result = await reviewService.UpdateReviewAsync(
                id, dto.Text, dto.Rating);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await reviewService.DeleteReviewAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }
}