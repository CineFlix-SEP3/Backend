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
        if (runTime <= 0)
            throw new ArgumentException("RunTime must be positive.");

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

        try
        {
            var grpcResponse = await movieClient.CreateMovieAsync(request);
            return MapToDto(grpcResponse);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to create movie: {ex.Message}", ex);
        }
    }

    public async Task<MovieDto> GetMovieByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        try
        {
            var grpcResponse = await movieClient.GetMovieByIdAsync(new GetMovieByIdRequest { Id = id });
            return MapToDto(grpcResponse);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movie by ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<MovieDto> GetMovieByTitleAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        try
        {
            var grpcResponse = await movieClient.GetMovieByTitleAsync(new GetMovieByTitleRequest { Title = title });
            return MapToDto(grpcResponse);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movie by title '{title}': {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        try
        {
            var grpcResponse = await movieClient.GetAllMoviesAsync(new GetAllMoviesRequest());
            return grpcResponse.Movies.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get all movies: {ex.Message}", ex);
        }
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
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

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

        try
        {
            var grpcResponse = await movieClient.UpdateMovieAsync(request);
            return MapToDto(grpcResponse);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to update movie with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<List<MovieDto>> GetMoviesByGenreAsync(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Genre is required.");

        try
        {
            var grpcResponse = await movieClient.GetMoviesByGenreAsync(new GetMoviesByGenreRequest { Genre = genre });
            return grpcResponse.Movies.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movies by genre '{genre}': {ex.Message}", ex);
        }
    }

    public async Task<List<MovieDto>> GetMoviesByDirectorAsync(string director)
    {
        if (string.IsNullOrWhiteSpace(director))
            throw new ArgumentException("Director is required.");

        try
        {
            var grpcResponse = await movieClient.GetMoviesByDirectorAsync(new GetMoviesByDirectorRequest { Director = director });
            return grpcResponse.Movies.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movies by director '{director}': {ex.Message}", ex);
        }
    }

    public async Task<List<MovieDto>> GetMoviesByActorAsync(string actor)
    {
        if (string.IsNullOrWhiteSpace(actor))
            throw new ArgumentException("Actor is required.");

        try
        {
            var grpcResponse = await movieClient.GetMoviesByActorAsync(new GetMoviesByActorRequest { Actor = actor });
            return grpcResponse.Movies.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movies by actor '{actor}': {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        try
        {
            var response = await movieClient.DeleteMovieAsync(new DeleteMovieRequest { Id = id });
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete movie with ID {id}: {ex.Message}", ex);
        }
    }
}