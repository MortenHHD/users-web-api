using Microsoft.AspNetCore.Mvc;
using UsersApi.Models;
using UsersApi.Repositories;

namespace UsersApi.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Email and password are required.");
        }

        // Tjek om email allerede findes
        var existingUser = _userRepository.GetByEmail(dto.Email);
        if (existingUser != null)
        {
            return BadRequest("Email is already in use.");
        }

        // Simpel password hashing (i virkeligheden bør man bruge BCrypt el.lign., men lad os holde det simpelt)
        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));

        var newUser = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash
        };

        _userRepository.Add(newUser);

        return Ok(new { message = "User registered successfully", userId = newUser.Id });
    }
}

// DTO til at modtage data fra klienten
public class RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}