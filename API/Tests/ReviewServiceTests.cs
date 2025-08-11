using Xunit;
using Moq;
using API.Services;
using API.DTOs.Review;
using GrpcClient;
using System.Threading.Tasks;

public class ReviewServiceTests
{
    [Fact]
    public async Task CreateReviewAsync_ReturnsReviewDto_WhenValid()
    {
        var reviewClientMock = new Mock<ReviewClient>("http://localhost:9090");
        var grpcResponse = new ReviewResponse
        {
            Id = 1,
            MovieId = 2,
            UserId = 3,
            Text = "Great movie!",
            Rating = 8.0
        };
        reviewClientMock.Setup(c => c.CreateReviewAsync(It.IsAny<CreateReviewRequest>()))
            .ReturnsAsync(grpcResponse);

        var service = new ReviewService(reviewClientMock.Object);

        var result = await service.CreateReviewAsync(2, 3, "Great movie!", 8.0);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(8.0, result.Rating);
    }

    [Fact]
    public async Task CreateReviewAsync_ThrowsArgumentException_WhenRatingInvalid()
    {
        var reviewClientMock = new Mock<ReviewClient>("http://localhost:9090");
        var service = new ReviewService(reviewClientMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateReviewAsync(2, 3, "Bad rating", 11.0));
    }
}