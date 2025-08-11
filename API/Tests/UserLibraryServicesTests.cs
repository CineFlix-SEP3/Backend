using Xunit;
using Moq;
using API.Services;
using Userlibrary;
using System.Threading.Tasks;
using GrpcClient;

public class UserLibraryServiceTests
{
    [Fact]
    public async Task AddFavoriteAsync_ReturnsTrue_WhenValid()
    {
        var clientMock = new Mock<UserLibraryClient>("http://localhost:9090");
        clientMock.Setup(c => c.AddFavoriteAsync(It.IsAny<AddFavoriteRequest>()))
            .ReturnsAsync(new AddFavoriteResponse { Success = true });

        var service = new UserLibraryService(clientMock.Object);

        var result = await service.AddFavoriteAsync(1, 2);

        Assert.True(result);
    }

    [Fact]
    public async Task AddFavoriteAsync_ThrowsArgumentException_WhenUserIdInvalid()
    {
        var clientMock = new Mock<UserLibraryClient>("http://localhost:9090");
        var service = new UserLibraryService(clientMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddFavoriteAsync(0, 2));
    }
}