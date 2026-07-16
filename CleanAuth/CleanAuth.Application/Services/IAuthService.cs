using CleanAuth.Application.Common;
using CleanAuth.Application.DTOs;

namespace CleanAuth.Application.Services;


// An interface for authentication services.
// It defines methods for user registration and login, returning authentication results.

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<AuthResult> LoginAsync(LoginRequest request);
}