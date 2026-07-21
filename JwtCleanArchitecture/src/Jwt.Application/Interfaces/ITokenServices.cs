using System.Security.Claims;
using Jwt.Domain.Entities;

namespace Jwt.Application.Interfaces;

public interface ITokenServices
{
   string GenerateAccessToken(User user);
   string GenerateRefreshToken();
    ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);

}