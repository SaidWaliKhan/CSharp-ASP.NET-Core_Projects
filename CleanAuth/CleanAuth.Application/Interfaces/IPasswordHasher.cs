namespace CleanAuth.Application.Interfaces;

// An interface for password hashing operations.
// It defines methods for hashing passwords and verifying hashed passwords.
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string PasswordHash);
}
