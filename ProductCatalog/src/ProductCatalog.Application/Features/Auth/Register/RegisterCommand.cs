using MediatR;
using ProductCatalog.Application.Common.Results;

namespace ProductCatalog.Application.Features.Auth.Register;

public sealed record RegisterCommand(
    string Name,
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;