using GrpcClient;

namespace API.Services;

public class MovieService(MovieClient movieClient)
{
    public async Task<MovieResponse> CreateMovieAsync(
        string title,
        IEnumerable<string> genres,
        IEnumerable<string> directors,
        IEnumerable<string> actors,
        int runTime,
        string releaseDate,
        double rating,
        string description,
        string posterUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");
        if (genres == null || !genres.Any())
            throw new ArgumentException("At least one genre is required.");
        if (directors == null || !directors.Any())
            throw new ArgumentException("At least one director is required.");
        if (actors == null || !actors.Any())
            throw new ArgumentException("At least one actor is required.");
        if (runTime <= 0)
            throw new ArgumentException("Run time must be positive.");
        if (string.IsNullOrWhiteSpace(releaseDate))
            throw new ArgumentException("Release date is required.");
        if (rating < 0 || rating > 10)
            throw new ArgumentException("Rating must be between 0 and 10.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.");
        if (string.IsNullOrWhiteSpace(posterUrl))
            throw new ArgumentException("Poster URL is required.");

        var request = new CreateMovieRequest
        {
            Title = title,
            Genres = { genres },
            Directors = { directors },
            Actors = { actors },
            RunTime = runTime,
            ReleaseDate = releaseDate,
            Rating = rating,
            Description = description,
            PosterUrl = posterUrl
        };

        try
        {
            return await movieClient.CreateMovieAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to create movie: {ex.Message}", ex);
        }
    }

    public async Task<MovieResponse> GetMovieByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var request = new GetMovieByIdRequest { Id = id };

        try
        {
            return await movieClient.GetMovieByIdAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movie by ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<MovieResponse> GetMovieByTitleAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        var request = new GetMovieByTitleRequest { Title = title };

        try
        {
            return await movieClient.GetMovieByTitleAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movie by title '{title}': {ex.Message}", ex);
        }
    }

    public async Task<GetAllMoviesResponse> GetAllMoviesAsync()
    {
        var request = new GetAllMoviesRequest();

        try
        {
            return await movieClient.GetAllMoviesAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get all movies: {ex.Message}", ex);
        }
    }

    public async Task<MovieResponse> UpdateMovieAsync(
        int id,
        string? title,
        IEnumerable<string>? genres,
        IEnumerable<string>? directors,
        IEnumerable<string>? actors,
        int? runTime,
        string? releaseDate,
        double? rating,
        string? description,
        string? posterUrl)
    {
        var existingMovie = await movieClient.GetMovieByIdAsync(new GetMovieByIdRequest { Id = id });
        if (existingMovie == null)
            throw new ArgumentException("Movie not found.");

        var request = new UpdateMovieRequest
        {
            Id = id,
            Title = title ?? existingMovie.Title,
            Genres = { genres ?? existingMovie.Genres },
            Directors = { directors ?? existingMovie.Directors },
            Actors = { actors ?? existingMovie.Actors },
            RunTime = runTime ?? existingMovie.RunTime,
            ReleaseDate = releaseDate ?? existingMovie.ReleaseDate,
            Rating = rating ?? existingMovie.Rating,
            Description = description ?? existingMovie.Description,
            PosterUrl = posterUrl ?? existingMovie.PosterUrl
        };

        try
        {
            return await movieClient.UpdateMovieAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to update movie with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<DeleteMovieResponse> DeleteMovieAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var request = new DeleteMovieRequest { Id = id };

        try
        {
            return await movieClient.DeleteMovieAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete movie with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<GetAllMoviesResponse> GetMoviesByGenreAsync(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Genre is required.");

        var request = new GetMoviesByGenreRequest { Genre = genre };

        try
        {
            return await movieClient.GetMoviesByGenreAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movies by genre '{genre}': {ex.Message}", ex);
        }
    }

    public async Task<GetAllMoviesResponse> GetMoviesByDirectorAsync(string director)
    {
        if (string.IsNullOrWhiteSpace(director))
            throw new ArgumentException("Director is required.");

        var request = new GetMoviesByDirectorRequest { Director = director };

        try
        {
            return await movieClient.GetMoviesByDirectorAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movies by director '{director}': {ex.Message}", ex);
        }
    }

    public async Task<GetAllMoviesResponse> GetMoviesByActorAsync(string actor)
    {
        if (string.IsNullOrWhiteSpace(actor))
            throw new ArgumentException("Actor is required.");

        var request = new GetMoviesByActorRequest { Actor = actor };

        try
        {
            return await movieClient.GetMoviesByActorAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get movies by actor '{actor}': {ex.Message}", ex);
        }
    }
}