using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs;
using API.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class AuthService(UserService userService, IConfiguration config)
{
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await userService.GetUserByEmailAsync(loginRequest.Email);
        if (!VerifyPassword(loginRequest.Password, user.HashedPassword))
            return null;

        var token = await GenerateTokenAsync(user);
        return new LoginResponseDto
        {
            Token = token,
            Expiry = DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpiresInMinutes"]!))
        };
    }

    public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
    {
        UserResponse? existing = null;
        try
        {
            existing = await userService.GetUserByUsernameAsync(createUserDto.Username);
        }
        catch (ApplicationException)
        {
        }

        if (existing != null)
            throw new InvalidOperationException("Auth already exists.");

        var user = await userService.CreateUserAsync(
            createUserDto.Username, createUserDto.Email, createUserDto.Password);

        return new UserDto(user.Id, user.Username, user.Email, user.UserRole);
    }

    private Task<string> GenerateTokenAsync(UserResponse user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole.ToUpperInvariant()) // Normalize to upper case
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpiresInMinutes"]!)),
            signingCredentials: creds
        );
        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    private bool VerifyPassword(string plainPassword, string? hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}