namespace GrpcClient;

public class UserClient : BaseGrpcClient
{
    private readonly UserService.UserServiceClient _client;

    public UserClient(string address) : base(address)
    {
        _client = new UserService.UserServiceClient(Channel);
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        => await _client.CreateUserAsync(request);

    public async Task<UserResponse> GetUserByEmailAsync(GetUserByEmailRequest request)
        => await _client.GetUserByEmailAsync(request);

    public async Task<UserResponse> GetUserByUsernameAsync(GetUserByUsernameRequest request)
        => await _client.GetUserByUsernameAsync(request);

    public async Task<UserResponse> GetUserByIdAsync(GetUserByIdRequest request)
        => await _client.GetUserByIdAsync(request);

    public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
        => await _client.UpdateUserAsync(request);

    public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
        => await _client.DeleteUserAsync(request);

    public async Task<GetAllUsersResponse> GetAllUsersAsync(GetAllUsersRequest request)
        => await _client.GetAllUsersAsync(request);
}