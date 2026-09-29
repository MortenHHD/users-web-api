using UsersApi.Models;

namespace UsersApi.Repositories;

public interface IUserRepository
{
    User? GetByEmail(string email);
    void Add(User user);
}