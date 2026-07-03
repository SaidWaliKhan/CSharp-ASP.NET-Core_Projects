using CleanAuth.Domain.Entities;

namespace CleanAuth.Application.Interfaces;

// An interface for user repository operations
// the repository pattern is used to abstract the data access layer and provide a clean interface for interacting with user data.
// it is simply a contract that defines the methods that a user repository should implement, without specifying how those methods are implemented.
public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
}