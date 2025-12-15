using Microsoft.EntityFrameworkCore;

[Keyless]
public class UserLoginReadModel
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
}