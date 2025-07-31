namespace API.DTOs.Auth;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;

    public UserDto() {}

    public UserDto(int id, string username, string email, string userRole)
    {
        Id = id;
        Username = username;
        Email = email;
        UserRole = userRole;
    }
}