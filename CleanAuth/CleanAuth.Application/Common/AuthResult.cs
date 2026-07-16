using CleanAuth.Application.DTOs;

namespace CleanAuth.Application.Common;

// A class representing the result of an authentication operation, including success status, error message, and authentication response data
// it gives us a clear way to handle the outcome of authentication operations, whether successful or failed, and provides relevant information accordingly.
// without throwing any exceptions, making it easier to manage authentication flow in the application.
public class AuthResult
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    public AuthResponse? Data { get; private set; }



    // A static method to create a successful AuthResult with the provided AuthResponse data
    public static AuthResult Ok(AuthResponse data)
    {
        return new AuthResult
        {
            Success = true,
            Data = data
        };
    }


    // A static method to create a failed AuthResult with the provided error message
    public static AuthResult Failure(string errorMessage)
    {
        return new AuthResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }

}