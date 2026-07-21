using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Jwt.Application.Interfaces;
using Jwt.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Jwt.Infrastructure.Services;

public class TokenService : ITokenServices
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]
        ?? throw new InvalidOperationException("SecretKey is missing in thr Configuration file");


        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };


        var SymmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentilas = new SigningCredentials(SymmetricKey, SecurityAlgorithms.HmacSha256);


        var expiresInMinutes = double.Parse(jwtSettings["AccessTokenExpirationOnMinutes"] ?? "15");


        var token = new JwtSecurityToken(

            issuer: jwtSettings["issuer"],
            audience: jwtSettings["audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentilas

        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    


    public string GenerateRefreshToken()
    {
        var randomBytes = new Byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);

    }

    public ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]
        ?? throw new InvalidOperationException("secret key is missing in configuration ");


        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false

        };

        var tokenHanlder = new JwtSecurityTokenHandler();
        var principal = tokenHanlder.ValidateToken(
        token,
        tokenValidationParameters,
        out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
        !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
        StringComparison.InvariantCultureIgnoreCase))

        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }
}