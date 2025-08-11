using Xunit;
using Moq;
using API.Services;
using API.DTOs.Movie;
using GrpcClient;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MovieServiceTests
{
    [Fact]
    public async Task CreateMovieAsync_ReturnsMovieDto_WhenValid()
    {
        var movieClientMock = new Mock<MovieClient>("http://localhost:9090");
        var grpcResponse = new MovieResponse
        {
            Id = 1,
            Title = "Test Movie",
            Genres = { "Action" },
            Directors = { "Director" },
            Actors = { "Actor" },
            RunTime = 120,
            ReleaseDate = "2024-01-01",
            Rating = 8.5,
            Description = "Desc",
            PosterUrl = "url"
        };
        movieClientMock.Setup(m => m.CreateMovieAsync(It.IsAny<CreateMovieRequest>()))
            .ReturnsAsync(grpcResponse);

        var service = new MovieService(movieClientMock.Object);

        var result = await service.CreateMovieAsync(
            "Test Movie",
            new[] { "Action" },
            new[] { "Director" },
            new[] { "Actor" },
            120,
            "2024-01-01",
            "Desc",
            "url"
        );

        Assert.NotNull(result);
        Assert.Equal("Test Movie", result.Title);
    }

    [Fact]
    public async Task CreateMovieAsync_ThrowsArgumentException_WhenTitleIsEmpty()
    {
        var movieClientMock = new Mock<MovieClient>("http://localhost:9090");
        var service = new MovieService(movieClientMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateMovieAsync(
                "",
                new[] { "Action" },
                new[] { "Director" },
                new[] { "Actor" },
                120,
                "2024-01-01",
                "Desc",
                "url"
            ));
    }
}