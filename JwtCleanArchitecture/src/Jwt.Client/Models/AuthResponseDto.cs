namespace Jwt.Client.Models;

// Mirrors the API's AuthResponseDto shape exactly - this is the contract between client and server.
public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }
}