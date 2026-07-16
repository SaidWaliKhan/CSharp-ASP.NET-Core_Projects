using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanAuth.Application.Interfaces;
using CleanAuth.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanAuth.Infrastructure.Security;

// Generates JWT tokens for authenticated users
// This class implements the IJwtTokenGenerator interface and provides functionality to generate JWT tokens based on user information and JWT settings.
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSetting)
    {
        _jwtSettings = jwtSetting.Value;
    }


    public string GenerateToken(User user)
    {
        // Create claims for the JWT token based on user information
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };


        // Create a symmetric security key using the secret from JWT settings.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        // Create signing credentials using the security key and HMAC SHA256 algorithm.
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



        // Create a JWT token using the claims, issuer, audience, expiration time, and signing credentials.
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        // Return the serialized JWT token as a string.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}