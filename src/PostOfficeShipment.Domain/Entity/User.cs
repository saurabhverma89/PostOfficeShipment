namespace PostOfficeShipment.Domain.Entities;

public class User
{
    public int Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = "User";

    public DateTime CreatedAt { get; private set; }

    private User()
    {
    }

    public User(string username, string passwordHash, string role = "User")
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        Username = username;
        PasswordHash = passwordHash;
        Role = string.IsNullOrWhiteSpace(role) ? "User" : role;

        CreatedAt = DateTime.UtcNow;
    }
}