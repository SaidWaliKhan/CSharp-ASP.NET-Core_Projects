using CleanAuth.Application.Interfaces;
using CleanAuth.Domain.Entities;

namespace CleanAuth.Infrastructure.Persistence;


// A repository class for managing User entities in memory.
// It provides methods to retrieve and add users based on their email addresses.
// The repository uses a thread-safe approach to ensure that concurrent access to the user list is handled correctly.
public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private readonly object _lock = new();


    // Retrieves a user by their email address asynchronously.
    // The method uses a lock to ensure thread safety during the search operation.
    public Task<User?> GetUserByEmailAsync(string email)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

    }


    public Task AddUserAsync(User user)
    {
        lock (_lock)
        {
            _users.Add(user);
        }
        return Task.CompletedTask;
    }


}







//using CleanAuth.Application.Interfaces;
//using CleanAuth.Domain.Entities;

//namespace CleanAuth.Infrastructure.Persistence;

// This is our "fake database" — just a List living in memory.
// We register this as a SINGLETON (one instance for the whole app's lifetime)
// so the list survives between requests. Restarting the app wipes it — that's expected.
/*public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private readonly object _lock = new();

    public Task<User?> GetByEmailAsync(string email)
    {
        lock (_lock)
        {
            User? foundUser = null;

            foreach (var user in _users)
            {
                if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    foundUser = user;
                    break;
                }
            }

            return Task.FromResult(foundUser);
        }
    }

    public Task AddAsync(User user)
    {
        lock (_lock)
        {
            _users.Add(user);
        }

        return Task.CompletedTask;
    }
}
*/