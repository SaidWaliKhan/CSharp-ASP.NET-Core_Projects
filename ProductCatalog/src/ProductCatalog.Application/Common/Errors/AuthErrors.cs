using ProductCatalog.Application.Common.Errors;

namespace ProductCatalog.Application.Common.Results;

public static class AuthErrors
{
    public static Error InvalidCredentials() =>
        new("Auth.InvalidCredentials", "Invalid email or password.");

    public static Error EmailAlreadyExists(string email) =>
        new("Auth.EmailAlreadyExists", $"Email '{email}' is already registered.");

    public static Error WeakPassword() =>
        new("Auth.WeakPassword", "Password must be at least 6 characters.");
}