namespace API.DTOs.Movie;

public class UpdateMovieDto
{
    public string? Title { get; set; }
    public IEnumerable<string>? Genres { get; set; }
    public IEnumerable<string>? Directors { get; set; }
    public IEnumerable<string>? Actors { get; set; }
    public int? RunTime { get; set; }
    public string? ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string? PosterUrl { get; set; }
}