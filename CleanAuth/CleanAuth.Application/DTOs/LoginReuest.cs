namespace CleanAuth.Application.DTOs;

// A login request DTO for user authentication
public record LoginRequest(
    string Email,
    string Password
);
