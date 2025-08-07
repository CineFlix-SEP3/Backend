using GrpcClient;
using Userlibrary;

namespace API.Services;

public class UserLibraryService(UserLibraryClient userLibraryClient)
{
    public async Task<bool> AddFavoriteAsync(int userId, int movieId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        if (movieId <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var request = new AddFavoriteRequest
        {
            UserId = userId,
            MovieId = movieId
        };

        try
        {
            var response = await userLibraryClient.AddFavoriteAsync(request);
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to add favorite: {ex.Message}", ex);
        }
    }

    public async Task<List<int>> GetFavoritesAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        var request = new GetFavoritesRequest
        {
            UserId = userId
        };

        try
        {
            var response = await userLibraryClient.GetFavoritesAsync(request);
            return response.MovieIds.ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get favorites for user ID {userId}: {ex.Message}", ex);
        }
    }

    public async Task<bool> RemoveFavoriteAsync(int userId, int movieId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        if (movieId <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var request = new RemoveFavoriteRequest
        {
            UserId = userId,
            MovieId = movieId
        };

        try
        {
            var response = await userLibraryClient.RemoveFavoriteAsync(request);
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to remove favorite: {ex.Message}", ex);
        }
    }

    public async Task<bool> AddWatchListAsync(int userId, int movieId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        if (movieId <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var request = new AddWatchListRequest
        {
            UserId = userId,
            MovieId = movieId
        };

        try
        {
            var response = await userLibraryClient.AddWatchListAsync(request);
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to add to watchlist: {ex.Message}", ex);
        }
    }

    public async Task<List<int>> GetWatchListAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        var request = new GetWatchListRequest
        {
            UserId = userId
        };

        try
        {
            var response = await userLibraryClient.GetWatchListAsync(request);
            return response.MovieIds.ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get watchlist for user ID {userId}: {ex.Message}", ex);
        }
    }

    public async Task<bool> RemoveWatchListAsync(int userId, int movieId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        if (movieId <= 0)
            throw new ArgumentException("Invalid movie ID.");

        var request = new RemoveWatchListRequest
        {
            UserId = userId,
            MovieId = movieId
        };

        try
        {
            var response = await userLibraryClient.RemoveWatchListAsync(request);
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to remove from watchlist: {ex.Message}", ex);
        }
    }
}