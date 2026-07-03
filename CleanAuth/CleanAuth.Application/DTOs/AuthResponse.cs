namespace CleanAuth.Application.DTOs;

// A response DTO for user authentication containing the JWT token and user email
public record AuthResponse(
    string Token,
    string Email
);
