using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "USER")]
public class UserLibraryController(API.Services.UserLibraryService userLibraryService) : ControllerBase
{
    [HttpPost("favorite/{movieId:int}")]
    public async Task<IActionResult> AddFavorite(int movieId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("Invalid user ID.");

        try
        {
            var success = await userLibraryService.AddFavoriteAsync(userId, movieId);
            return Ok(success);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpPost("watchlist/{movieId:int}")]
    public async Task<IActionResult> AddWatchList(int movieId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("Invalid user ID.");

        try
        {
            var success = await userLibraryService.AddWatchListAsync(userId, movieId);
            return Ok(success);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpGet("favorite")]
    public async Task<IActionResult> GetFavorites()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("Invalid user ID.");

        try
        {
            var favorites = await userLibraryService.GetFavoritesAsync(userId);
            return Ok(favorites);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpGet("watchlist")]
    public async Task<IActionResult> GetWatchList()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("Invalid user ID.");

        try
        {
            var watchlist = await userLibraryService.GetWatchListAsync(userId);
            return Ok(watchlist);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpDelete("favorite/{movieId:int}")]
    public async Task<IActionResult> RemoveFavorite(int movieId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("Invalid user ID.");

        try
        {
            var success = await userLibraryService.RemoveFavoriteAsync(userId, movieId);
            return Ok(success);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }

    [HttpDelete("watchlist/{movieId:int}")]
    public async Task<IActionResult> RemoveWatchList(int movieId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("Invalid user ID.");

        try
        {
            var success = await userLibraryService.RemoveWatchListAsync(userId, movieId);
            return Ok(success);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (ApplicationException ex) { return StatusCode(500, ex.Message); }
    }
}