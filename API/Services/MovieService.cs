using API.DTOs.Movie;
using GrpcClient;

namespace API.Services;

public class MovieService(MovieClient movieClient)
{
    private static MovieDto MapToDto(MovieResponse movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Genres = movie.Genres.ToList(),
            Directors = movie.Directors.ToList(),
            Actors = movie.Actors.ToList(),
            RunTime = movie.RunTime,
            ReleaseDate = movie.ReleaseDate,
            Rating = movie.Rating,
            Description = movie.Description,
            PosterUrl = movie.PosterUrl
        };
    }

    public async Task<MovieDto> CreateMovieAsync(
        string title,
        IEnumerable<string> genres,
        IEnumerable<string> directors,
        IEnumerable<string> actors,
        int runTime,
        string releaseDate,
        string description,
        string posterUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        var uniqueGenres = genres.Distinct().ToList();
        var uniqueDirectors = directors.Distinct().ToList();
        var uniqueActors = actors.Distinct().ToList();

        var request = new CreateMovieRequest
        {
            Title = title,
            Genres = { uniqueGenres },
            Directors = { uniqueDirectors },
            Actors = { uniqueActors },
            RunTime = runTime,
            ReleaseDate = releaseDate,
            Description = description,
            PosterUrl = posterUrl
        };

        var grpcResponse = await movieClient.CreateMovieAsync(request);
        return MapToDto(grpcResponse);
    }

    public async Task<MovieDto> GetMovieByIdAsync(int id)
    {
        var grpcResponse = await movieClient.GetMovieByIdAsync(new GetMovieByIdRequest { Id = id });
        return MapToDto(grpcResponse);
    }

    public async Task<MovieDto> GetMovieByTitleAsync(string title)
    {
        var grpcResponse = await movieClient.GetMovieByTitleAsync(new GetMovieByTitleRequest { Title = title });
        return MapToDto(grpcResponse);
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var grpcResponse = await movieClient.GetAllMoviesAsync(new GetAllMoviesRequest());
        return grpcResponse.Movies.Select(MapToDto).ToList();
    }

    public async Task<MovieDto> UpdateMovieAsync(
        int id,
        string? title,
        IEnumerable<string>? genres,
        IEnumerable<string>? directors,
        IEnumerable<string>? actors,
        int? runTime,
        string? releaseDate,
        string? description,
        string? posterUrl)
    {
        var uniqueGenres = genres?.Distinct().ToList() ?? new List<string>();
        var uniqueDirectors = directors?.Distinct().ToList() ?? new List<string>();
        var uniqueActors = actors?.Distinct().ToList() ?? new List<string>();

        var request = new UpdateMovieRequest
        {
            Id = id,
            Title = title ?? "",
            Genres = { uniqueGenres },
            Directors = { uniqueDirectors },
            Actors = { uniqueActors },
            RunTime = runTime ?? 0,
            ReleaseDate = releaseDate ?? "",
            Description = description ?? "",
            PosterUrl = posterUrl ?? ""
        };

        var grpcResponse = await movieClient.UpdateMovieAsync(request);
        return MapToDto(grpcResponse);
    }

    public async Task<List<MovieDto>> GetMoviesByGenreAsync(string genre)
    {
        var grpcResponse = await movieClient.GetMoviesByGenreAsync(new GetMoviesByGenreRequest { Genre = genre });
        return grpcResponse.Movies.Select(MapToDto).ToList();
    }

    public async Task<List<MovieDto>> GetMoviesByDirectorAsync(string director)
    {
        var grpcResponse = await movieClient.GetMoviesByDirectorAsync(new GetMoviesByDirectorRequest { Director = director });
        return grpcResponse.Movies.Select(MapToDto).ToList();
    }

    public async Task<List<MovieDto>> GetMoviesByActorAsync(string actor)
    {
        var grpcResponse = await movieClient.GetMoviesByActorAsync(new GetMoviesByActorRequest { Actor = actor });
        return grpcResponse.Movies.Select(MapToDto).ToList();
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        var response = await movieClient.DeleteMovieAsync(new DeleteMovieRequest { Id = id });
        return response.Success;
    }
}