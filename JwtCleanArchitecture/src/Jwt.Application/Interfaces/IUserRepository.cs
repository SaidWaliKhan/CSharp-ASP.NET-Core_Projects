using Jwt.Domain.Entities;

namespace Jwt.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);

    Task<User?> GetByIdAsync(Guid id);

    Task AddUserAsync(User user);

    Task UpdateAsync(User user);

    Task AddRefreshTokenAsync(RefreshToken refreshToken);

    Task SaveChangesAsync();
}