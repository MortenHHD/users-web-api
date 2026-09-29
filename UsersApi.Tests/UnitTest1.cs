using UsersApi.Models;
using UsersApi.Repositories;
using Xunit;

namespace UsersApi.Tests;

public class UserRepositoryTests
{
    [Fact]
    public void Add_And_GetByEmail_ShouldReturnUser()
    {
        // Arrange (Opsætning)
        var repository = new UserRepository();
        var user = new User
        {
            Username = "TestMorten",
            Email = "morten@test.dk",
            PasswordHash = "securepassword123"
        };

        // Act (Handling)
        repository.Add(user);
        var retrievedUser = repository.GetByEmail("morten@test.dk");

        // Assert (Bekræftelse)
        Assert.NotNull(retrievedUser);
        Assert.Equal("TestMorten", retrievedUser.Username);
        Assert.Equal("morten@test.dk", retrievedUser.Email);
    }
}