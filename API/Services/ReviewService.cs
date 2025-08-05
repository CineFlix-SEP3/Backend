using GrpcClient;
using API.DTOs.Review;

namespace API.Services;

public class ReviewService(ReviewClient reviewClient)
{
    private static ReviewDto MapToDto(ReviewResponse review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            MovieId = review.MovieId,
            UserId = review.UserId,
            Text = review.Text,
            Rating = review.Rating
        };
    }

    public async Task<ReviewDto> CreateReviewAsync(int movieId, int userId, string text, double rating)
    {
        if (movieId <= 0)
            throw new ArgumentException("Invalid movie ID.");
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Review text is required.");
        if (rating < 0.0 || rating > 10.0)
            throw new ArgumentException("Rating must be between 0 and 10.");

        var request = new CreateReviewRequest
        {
            MovieId = movieId,
            UserId = userId,
            Text = text,
            Rating = rating
        };

        try
        {
            var response = await reviewClient.CreateReviewAsync(request);
            return MapToDto(response);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to create review: {ex.Message}", ex);
        }
    }

    public async Task<List<ReviewDto>> GetReviewsByMovieAsync(int movieId)
    {
        if (movieId <= 0)
            throw new ArgumentException("Invalid movie ID.");

        try
        {
            var request = new GetReviewsByMovieRequest { MovieId = movieId };
            var response = await reviewClient.GetReviewsByMovieAsync(request);
            return response.Reviews.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get reviews for movie ID {movieId}: {ex.Message}", ex);
        }
    }

    public async Task<List<ReviewDto>> GetReviewsByUserAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        try
        {
            var request = new GetReviewsByUserRequest { UserId = userId };
            var response = await reviewClient.GetReviewsByUserAsync(request);
            return response.Reviews.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get reviews for user ID {userId}: {ex.Message}", ex);
        }
    }

    public async Task<ReviewDto> UpdateReviewAsync(int id, string text, double rating)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid review ID.");
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Review text is required.");
        if (rating < 0.0 || rating > 10.0)
            throw new ArgumentException("Rating must be between 0 and 10.");

        var request = new UpdateReviewRequest
        {
            Id = id,
            Text = text,
            Rating = rating
        };

        try
        {
            var response = await reviewClient.UpdateReviewAsync(request);
            return MapToDto(response);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to update review with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteReviewAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid review ID.");

        try
        {
            var request = new DeleteReviewRequest { Id = id };
            var response = await reviewClient.DeleteReviewAsync(request);
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete review with ID {id}: {ex.Message}", ex);
        }
    }
}