namespace API.DTOs.Movie;

public class CreateMovieDto
{
    public string Title { get; set; } = string.Empty;
    public IEnumerable<string> Genres { get; set; } = Array.Empty<string>();
    public IEnumerable<string> Directors { get; set; } = Array.Empty<string>();
    public IEnumerable<string> Actors { get; set; } = Array.Empty<string>();
    public int RunTime { get; set; }
    public string ReleaseDate { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
}