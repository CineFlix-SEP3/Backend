using System.Threading.Tasks;

namespace GrpcClient;

public class ReviewClient : BaseGrpcClient
{
    private readonly ReviewService.ReviewServiceClient _client;

    public ReviewClient(string address) : base(address)
    {
        _client = new ReviewService.ReviewServiceClient(Channel);
    }

    public async Task<ReviewResponse> CreateReviewAsync(CreateReviewRequest request)
        => await _client.CreateReviewAsync(request);

    public async Task<GetAllReviewsResponse> GetReviewsByMovieAsync(GetReviewsByMovieRequest request)
        => await _client.GetReviewsByMovieAsync(request);

    public async Task<GetAllReviewsResponse> GetReviewsByUserAsync(GetReviewsByUserRequest request)
        => await _client.GetReviewsByUserAsync(request);

    public async Task<ReviewResponse> UpdateReviewAsync(UpdateReviewRequest request)
        => await _client.UpdateReviewAsync(request);

    public async Task<DeleteReviewResponse> DeleteReviewAsync(DeleteReviewRequest request)
        => await _client.DeleteReviewAsync(request);
}