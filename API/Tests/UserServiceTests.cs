using Xunit;
using Moq;
using API.Services;
using API.DTOs.Auth;
using GrpcClient;
using System.Threading.Tasks;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUserAsync_ReturnsUserDto_WhenValid()
    {
        var userClientMock = new Mock<UserClient>("http://localhost:9090");
        var grpcResponse = new UserResponse
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            HashedPassword = "hashed",
            UserRole = "User"
        };
        userClientMock.Setup(c => c.CreateUserAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(grpcResponse);

        var service = new UserService(userClientMock.Object);

        var result = await service.CreateUserAsync("testuser", "test@example.com", "password123");

        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task CreateUserAsync_ThrowsArgumentException_WhenUsernameEmpty()
    {
        var userClientMock = new Mock<UserClient>("http://localhost:9090");
        var service = new UserService(userClientMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateUserAsync("", "test@example.com", "password123"));
    }
}