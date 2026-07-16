using CleanAuth.Application.Common;
using CleanAuth.Application.DTOs;
using CleanAuth.Application.Interfaces;
using CleanAuth.Domain.Entities;

namespace CleanAuth.Application.Services;

// An implementation of the IAuthService interface for user authentication and registration.
// It handles user registration, login, password hashing, and JWT token generation.
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;


    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }


    // Registers a new user with the provided registration request.
    // It checks if the user already exists, hashes the password, creates a new user entity, adds it to the repository, and generates a JWT token for the new user.

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email;
        var password = request.Password;

        // Check if the user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            return AuthResult.Failure("User with this email already exists.");
        }

        // Hash the password
        var passwordHash = _passwordHasher.HashPassword(password);

        // Create a new user entity
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash
        };

        // Add the new user to the repository
        await _userRepository.AddUserAsync(newUser);

        // Generate a JWT token for the new user
        var token = _jwtTokenGenerator.GenerateToken(newUser);

        return AuthResult.Ok(new AuthResponse(token, email));
    }

    // Logs in a user with the provided login request.
    // It retrieves the user by email, verifies the password, and generates a JWT token for
    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var email = request.Email;
        var password = request.Password;

        // Retrieve the user by email
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return AuthResult.Failure("Invalid email or password.");
        }

        // Verify the password
        bool isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return AuthResult.Failure("Invalid email or password.");
        }

        // Generate a JWT token for the authenticated user
        var token = _jwtTokenGenerator.GenerateToken(user);

        return AuthResult.Ok(new AuthResponse(token, email));



    }

}