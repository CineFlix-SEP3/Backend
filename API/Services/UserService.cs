using System.Security.Cryptography;
using System.Text;
using GrpcClient;

namespace API.Services;

public class UserService(UserClient userClient)
{
    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public async Task<UserResponse> CreateUserAsync(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");

        var userRole = email == "admin@gmail.com" ? "Admin" : "Auth";
        var hashedPassword = HashPassword(password);

        var request = new CreateUserRequest
        {
            Username = username,
            Email = email,
            HashedPassword = hashedPassword,
            UserRole = userRole
        };

        try
        {
            return await userClient.CreateUserAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to create user: {ex.Message}", ex);
        }
    }

    public async Task<UserResponse> GetUserByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID.");

        var request = new GetUserByIdRequest { Id = id };

        try
        {
            return await userClient.GetUserByIdAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get user by ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<UserResponse> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        var request = new GetUserByEmailRequest { Email = email };

        try
        {
            return await userClient.GetUserByEmailAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get user by email '{email}': {ex.Message}", ex);
        }
    }

    public async Task<UserResponse> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        var request = new GetUserByUsernameRequest { Username = username };

        try
        {
            return await userClient.GetUserByUsernameAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get user by username '{username}': {ex.Message}", ex);
        }
    }

    public async Task<UserResponse> UpdateUserAsync(int id, string? username, string? email, string? password, string? userRole)
    {
        var existingUser = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = id });

        if (existingUser == null)
            throw new ArgumentException("Auth not found.");

        var updatedUsername = username ?? existingUser.Username;
        var updatedEmail = email ?? existingUser.Email;
        var updatedPassword = password != null ? HashPassword(password) : existingUser.HashedPassword;
        var updatedUserRole = userRole ?? existingUser.UserRole;

        var request = new UpdateUserRequest
        {
            Id = id,
            Username = updatedUsername,
            Email = updatedEmail,
            HashedPassword = updatedPassword,
            UserRole = updatedUserRole
        };

        try
        {
            return await userClient.UpdateUserAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to update user with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<DeleteUserResponse> DeleteUserAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID.");

        var request = new DeleteUserRequest { Id = id };

        try
        {
            return await userClient.DeleteUserAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete user with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<GetAllUsersResponse> GetAllUsersAsync()
    {
        var request = new GetAllUsersRequest();

        try
        {
            return await userClient.GetAllUsersAsync(request);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get all users: {ex.Message}", ex);
        }
    }
}