namespace CleanAuth.Application.DTOs;

//  a register request DTO for user registration
public record RegisterRequest(
    string Email,
    string Password
);
