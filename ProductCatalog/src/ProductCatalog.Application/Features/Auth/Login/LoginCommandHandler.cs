using MediatR;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Application.Features.Auth.Register;

namespace ProductCatalog.Application.Features.Auth.Login;

public sealed class LoginCommandHandler(
    IUserRepository repository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponse>(AuthErrors.InvalidCredentials());

        var isValidPassword = passwordHasher.Verify(
            request.Password, user.PasswordHash
        );

        if (!isValidPassword)
            return Result.Failure<AuthResponse>(
                AuthErrors.InvalidCredentials());

        var token = jwtService.GenerateToken(user);

        var response = new AuthResponse(
            user.Id,
            user.Name,
            user.Email,
            token);

        return Result.Success(response);
    }
}