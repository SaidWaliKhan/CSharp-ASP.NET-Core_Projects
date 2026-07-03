using CleanAuth.Application.Interfaces;
using CleanAuth.Infrastructure.Persistence;
using CleanAuth.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAuth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register JwtSettings from configuration file
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Register PasswordHasher as a singleton service for password hashing
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // Regiter JwtTokenGenerator as a singleton service for generating JWT tokens
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Register UserRepository as a scoped service for user data access
        services.AddSingleton<IUserRepository, UserRepository>();


        return services;
    }
}