using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class AuthService(UserService userService, IConfiguration config)
{
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await userService.GetUserByEmailAsync(loginRequest.Email);
        var userGrpc = await userService.GetUserByEmailGrpcAsync(loginRequest.Email);
        if (!VerifyPassword(loginRequest.Password, userGrpc.HashedPassword))
            return null;

        var token = await GenerateTokenAsync(userGrpc);
        return new LoginResponseDto
        {
            Token = token,
            Expiry = DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpiresInMinutes"]!))
        };
    }

    public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
    {
        UserDto? existing = null;
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

        return user;
    }

    private Task<string> GenerateTokenAsync(UserResponse user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole.ToUpperInvariant())
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