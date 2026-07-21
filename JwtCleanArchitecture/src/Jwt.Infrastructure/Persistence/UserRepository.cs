using Jwt.Application.Interfaces;
using Jwt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jwt.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }


    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _db.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }


    public Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }


    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _db.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}