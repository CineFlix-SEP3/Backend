using API.DTOs.Auth;
using GrpcClient;

namespace API.Services;

public class UserService(UserClient userClient)
{
    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static UserDto MapToDto(UserResponse user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            UserRole = user.UserRole
        };
    }

    public async Task<UserDto> CreateUserAsync(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");

        var userRole = email == "admin@gmail.com" ? "Admin" : "User";
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
            var user = await userClient.CreateUserAsync(request);
            return MapToDto(user);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to create user: {ex.Message}", ex);
        }
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID.");

        var request = new GetUserByIdRequest { Id = id };

        try
        {
            var user = await userClient.GetUserByIdAsync(request);
            return MapToDto(user);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get user by ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        var request = new GetUserByEmailRequest { Email = email };

        try
        {
            var user = await userClient.GetUserByEmailAsync(request);
            return MapToDto(user);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get user by email '{email}': {ex.Message}", ex);
        }
    }
    
    public async Task<UserResponse> GetUserByEmailGrpcAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        var request = new GetUserByEmailRequest { Email = email };
        return await userClient.GetUserByEmailAsync(request);
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        var request = new GetUserByUsernameRequest { Username = username };

        try
        {
            var user = await userClient.GetUserByUsernameAsync(request);
            return MapToDto(user);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get user by username '{username}': {ex.Message}", ex);
        }
    }

    public async Task<UserDto> UpdateUserAsync(int id, string? username, string? email, string? password, string? userRole)
    {
        var existingUser = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = id });

        if (existingUser == null)
            throw new ArgumentException("Auth not found.");

        var updatedUsername = username ?? existingUser.Username;
        var updatedEmail = email ?? existingUser.Email;
        var updatedUserRole = userRole ?? existingUser.UserRole;

        var updatedPassword = !string.IsNullOrWhiteSpace(password)
            ? HashPassword(password)
            : existingUser.HashedPassword;

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
            var user = await userClient.UpdateUserAsync(request);
            return MapToDto(user);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to update user with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID.");

        var request = new DeleteUserRequest { Id = id };

        try
        {
            var response = await userClient.DeleteUserAsync(request);
            return response.Success;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete user with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var request = new GetAllUsersRequest();

        try
        {
            var response = await userClient.GetAllUsersAsync(request);
            return response.Users.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to get all users: {ex.Message}", ex);
        }
    }
}