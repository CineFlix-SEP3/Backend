namespace GrpcClient;

public class MovieClient : BaseGrpcClient
{
    private readonly MovieService.MovieServiceClient _client;

    public MovieClient(string address) : base(address)
    {
        _client = new MovieService.MovieServiceClient(Channel);
    }

    public async Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request)
        => await _client.CreateMovieAsync(request);

    public async Task<MovieResponse> GetMovieByIdAsync(GetMovieByIdRequest request)
        => await _client.GetMovieByIdAsync(request);

    public async Task<MovieResponse> GetMovieByTitleAsync(GetMovieByTitleRequest request)
        => await _client.GetMovieByTitleAsync(request);

    public async Task<GetAllMoviesResponse> GetAllMoviesAsync(GetAllMoviesRequest request)
        => await _client.GetAllMoviesAsync(request);

    public async Task<MovieResponse> UpdateMovieAsync(UpdateMovieRequest request)
        => await _client.UpdateMovieAsync(request);

    public async Task<DeleteMovieResponse> DeleteMovieAsync(DeleteMovieRequest request)
        => await _client.DeleteMovieAsync(request);

    public async Task<GetAllMoviesResponse> GetMoviesByGenreAsync(GetMoviesByGenreRequest request)
        => await _client.GetMoviesByGenreAsync(request);

    public async Task<GetAllMoviesResponse> GetMoviesByDirectorAsync(GetMoviesByDirectorRequest request)
        => await _client.GetMoviesByDirectorAsync(request);

    public async Task<GetAllMoviesResponse> GetMoviesByActorAsync(GetMoviesByActorRequest request)
        => await _client.GetMoviesByActorAsync(request);
}