using API.DTOs;
using API.DTOs.Auth;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
    {
        var response = await authService.LoginAsync(loginRequest);
        if (response == null)
            return Unauthorized("Invalid username or password.");

        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
    {
        if (string.IsNullOrWhiteSpace(createUserDto.Username) ||
            string.IsNullOrWhiteSpace(createUserDto.Email) ||
            string.IsNullOrWhiteSpace(createUserDto.Password))
        {
            return BadRequest("Username, email, and password are required.");
        }

        try
        {
            var user = await authService.RegisterAsync(createUserDto);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("test")]
    [Authorize]
    public IActionResult Test()
    {
        return Ok("You are authenticated!");
    }
}