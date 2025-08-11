using Xunit;
using Moq;
using API.Services;
using API.DTOs.Auth;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
    {
        var userServiceMock = new Mock<UserService>(null);
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkeysupersecretkey");
        configMock.Setup(c => c["Jwt:ExpiresInMinutes"]).Returns("60");
        configMock.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
        configMock.Setup(c => c["Jwt:Audience"]).Returns("audience");

        var userGrpc = new UserResponse
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password"),
            UserRole = "User"
        };

        userServiceMock.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserDto());
        userServiceMock.Setup(s => s.GetUserByEmailGrpcAsync(It.IsAny<string>()))
            .ReturnsAsync(userGrpc);

        var authService = new AuthService(userServiceMock.Object, configMock.Object);

        var result = await authService.LoginAsync(new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "password"
        });

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Token));
    }

    [Fact]
    public async Task LoginAsync_ReturnsNull_WhenPasswordIsInvalid()
    {
        var userServiceMock = new Mock<UserService>(null);
        var configMock = new Mock<IConfiguration>();

        var userGrpc = new UserResponse
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("password"),
            UserRole = "User"
        };

        userServiceMock.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserDto());
        userServiceMock.Setup(s => s.GetUserByEmailGrpcAsync(It.IsAny<string>()))
            .ReturnsAsync(userGrpc);

        var authService = new AuthService(userServiceMock.Object, configMock.Object);

        var result = await authService.LoginAsync(new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        });

        Assert.Null(result);
    }
}