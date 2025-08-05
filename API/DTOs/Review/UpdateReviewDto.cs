namespace API.DTOs.Review;

public class UpdateReviewDto
{
    public string Text { get; set; } = string.Empty;
    public double Rating { get; set; }
}