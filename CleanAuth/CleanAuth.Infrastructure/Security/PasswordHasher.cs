using CleanAuth.Application.Interfaces;

namespace CleanAuth.Infrastructure.Security;


// this class Hash the user password.
public class PasswordHasher : IPasswordHasher
{
    // Hashes the provided password using BCrypt and returns the hashed password.
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Verifies if the provided password matches the hashed password.
    public bool VerifyPassword(string password, string PasswordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}