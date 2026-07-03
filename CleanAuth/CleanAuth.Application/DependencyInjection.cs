using CleanAuth.Application.Interfaces;
using CleanAuth.Application.Services;
using Microsoft.Extensions.DependencyInjection;


namespace CleanAuth.Application;

// A static class that provides extension methods for registering application services and dependencies.
// The AddApplication method registers the IAuthService and its implementation AuthService with the dependency injection container.
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services and dependencies here
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}