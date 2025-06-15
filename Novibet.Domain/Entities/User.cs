namespace Novibet.Domain.Entities;
public class User
{
    //user entity with role , username, password (hashed)
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? Role { get; set; }
}
