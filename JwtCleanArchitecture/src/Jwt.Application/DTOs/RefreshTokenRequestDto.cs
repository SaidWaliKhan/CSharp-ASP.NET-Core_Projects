using System.ComponentModel.DataAnnotations;

namespace Jwt.Application.DTOs;

public record RefreshTokenRequestDto
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}