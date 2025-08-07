using Userlibrary;

namespace GrpcClient;

public class UserLibraryClient : BaseGrpcClient
{
    private readonly UserLibraryService.UserLibraryServiceClient _client;

    public UserLibraryClient(string address) : base(address)
    {
        _client = new UserLibraryService.UserLibraryServiceClient(Channel);
    }

    public async Task<AddFavoriteResponse> AddFavoriteAsync(AddFavoriteRequest request)
        => await _client.addFavoriteAsync(request);

    public async Task<GetFavoritesResponse> GetFavoritesAsync(GetFavoritesRequest request)
        => await _client.getFavoritesAsync(request);

    public async Task<RemoveFavoriteResponse> RemoveFavoriteAsync(RemoveFavoriteRequest request)
        => await _client.removeFavoriteAsync(request);

    public async Task<AddWatchListResponse> AddWatchListAsync(AddWatchListRequest request)
        => await _client.addWatchListAsync(request);

    public async Task<GetWatchListResponse> GetWatchListAsync(GetWatchListRequest request)
        => await _client.getWatchListAsync(request);

    public async Task<RemoveWatchListResponse> RemoveWatchListAsync(RemoveWatchListRequest request)
        => await _client.removeWatchListAsync(request);
}