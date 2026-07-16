using MediatR;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Application.Features.Auth.Register;

namespace ProductCatalog.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;