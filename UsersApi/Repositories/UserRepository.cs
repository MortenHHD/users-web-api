using UsersApi.Models;

namespace UsersApi.Repositories;

public class UserRepository : IUserRepository

{
    private readonly List<User> _users = new();

    public User? GetByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public void Add(User user)
    {
        user.Id = _users.Count + 1;
        _users.Add(user);
    }
}