using API.DTOs.Review;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [Authorize(Roles = "USER")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
                return Unauthorized("Invalid user ID.");

            var result = await reviewService.CreateReviewAsync(
                dto.MovieId, userId, dto.Text, dto.Rating);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize(Roles = "USER")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
                return Unauthorized("Invalid user ID.");

            var review = await reviewService.GetReviewByIdAsync(id);
            if (review == null || review.UserId != userId)
                return Forbid("You can only update your own review.");

            var result = await reviewService.UpdateReviewAsync(
                id, dto.Text, dto.Rating);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [Authorize(Roles = "USER")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
                return Unauthorized("Invalid user ID.");

            var review = await reviewService.GetReviewByIdAsync(id);
            if (review == null || review.UserId != userId)
                return Forbid("You can only delete your own review.");

            var result = await reviewService.DeleteReviewAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }
}