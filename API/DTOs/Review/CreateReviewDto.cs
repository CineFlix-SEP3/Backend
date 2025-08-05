namespace API.DTOs.Review;

public class CreateReviewDto
{
    public int MovieId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public double Rating { get; set; }
}