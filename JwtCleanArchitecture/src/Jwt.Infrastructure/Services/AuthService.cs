using System.IdentityModel.Tokens.Jwt;
using Jwt.Application.DTOs;
using Jwt.Application.Interfaces;
using Jwt.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Jwt.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenServices _tokenService;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthService(
        IUserRepository userRepository,
        ITokenServices tokenService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        return await GenerateAuthResultAsync(user);
    }



    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        try
        {
            var principal = _tokenService.GetClaimsPrincipalFromExpiredToken(request.AccessToken);

            var userIdClaim = principal?.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid access token."
                };
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var storedRefreshToken = user.RefreshTokens
                .FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (storedRefreshToken is null || !storedRefreshToken.IsActive)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired refresh token."
                };
            }

            storedRefreshToken.IsRevoked = true;

            return await GenerateAuthResultAsync(user);
        }
        catch (Exception ex)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = ex.Message
            };
        }
    }



    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "User already exists."
            };
        }

        var newUser = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Role = "User"
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(
            newUser,
            request.Password);

        await _userRepository.AddUserAsync(newUser);
        await _userRepository.SaveChangesAsync();

        return await GenerateAuthResultAsync(newUser);
    }



    private async Task<AuthResponseDto> GenerateAuthResultAsync(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshTokenExpirationDays = double.Parse(
            jwtSettings["RefreshTokenExpirationDays"] ?? "7");

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
            UserId = user.Id
        };


        user.RefreshTokens.Add(refreshToken);
        await _userRepository.AddRefreshTokenAsync(refreshToken);
        await _userRepository.SaveChangesAsync();

        var accessTokenExpirationMinutes = double.Parse(
            jwtSettings["AccessTokenExpirationMinutes"] ?? "15");


        return new AuthResponseDto
        {
            Success = true,
            Message = "Authentication successful.",
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes)
        };
    }
}