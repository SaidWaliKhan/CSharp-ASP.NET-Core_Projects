namespace AuthService.Models;

// This record represents a user in the authentication service.
public record User(
    Guid Id,
    string Username,
    string PasswordHash,
    string Role
);