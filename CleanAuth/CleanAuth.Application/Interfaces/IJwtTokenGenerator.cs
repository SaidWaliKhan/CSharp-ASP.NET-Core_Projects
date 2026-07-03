using CleanAuth.Domain.Entities;

namespace CleanAuth.Application.Interfaces;

// An interface for generating JWT tokens.
// It defines a method for generating a JWT token based on a user entity.
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}