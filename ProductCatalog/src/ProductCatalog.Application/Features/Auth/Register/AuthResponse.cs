namespace ProductCatalog.Application.Features.Auth.Register;

public sealed record AuthResponse(
    int Id,
    string Name,
    string Email,
    string Token);