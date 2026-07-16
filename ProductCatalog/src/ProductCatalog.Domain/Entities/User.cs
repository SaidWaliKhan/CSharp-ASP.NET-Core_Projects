using ProductCatalog.Domain.Exceptions;

namespace ProductCatalog.Domain.Entities;

public class User
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string Role { get; private set; }

    private User()
    {
        Name = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        Role = string.Empty;
    }

    public User(
        string name,
        string email,
        string passwordHash,
        string role = "User")
    {
        SetName(name);
        SetEmail(email);

        PasswordHash = passwordHash;

        Role = role;
    }

    public static User Create(string name, string email, string passwordHash, string role = "User")
    {
        return new User(name, email, passwordHash, role);
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void ChangeRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new DomainException("Role is required.");

        Role = role;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("User name is required.");

        Name = name.Trim();
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        Email = email.Trim().ToLowerInvariant();
    }
}