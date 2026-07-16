using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Simple in-memory user store (for demo only)
    private static readonly List<User> _users = new()
    {
        new User(Guid.NewGuid(), "ayyan", "password1", "Customer"),
        new User(Guid.NewGuid(), "admin", "password2", "Admin")
    };

    // to read configurations from appsetting.json file
    private readonly IConfiguration _config;

    public AuthController(IConfiguration configuration)
    {
        _config = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _users.SingleOrDefault(u => u.Username == request.Username && u.PasswordHash == request.Password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        // 1. Define the claims (data inside the token)
        var claims = new[]
        {
        
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // 2. Create the symmetric security key from the secret string
        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));

        // 3. Define the signing credentials (using HMAC SHA256)
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


        // 5. Create the actual JWT token object
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}