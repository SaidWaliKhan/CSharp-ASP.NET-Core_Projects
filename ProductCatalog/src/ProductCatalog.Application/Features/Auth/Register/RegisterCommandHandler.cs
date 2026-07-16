using MediatR;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Common.Results;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Features.Auth.Register;

public sealed class RegisterCommandHandler(
    IUserRepository repository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await repository
            .GetByEmailAsync(request.Email, cancellationToken);


        if (existing is not null)
            return Result.Failure<AuthResponse>(
                AuthErrors.EmailAlreadyExists(request.Email));


        var hash = passwordHasher.Hash(request.Password);


        var user = User.Create(
            request.Name,
            request.Email,
            hash);


        await repository.AddAsync(user, cancellationToken);

        await repository.SaveChangesAsync(cancellationToken);


        var token = jwtService.GenerateToken(user);


        return Result.Success(
            new AuthResponse(
                user.Id,
                user.Name,
                user.Email,
                token));
    }
}