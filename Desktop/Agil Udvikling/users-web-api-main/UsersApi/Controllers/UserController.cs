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

        var existingUser = _userRepository.GetByEmail(dto.Email);
        if (existingUser != null)
        {
            return BadRequest("Email is already in use.");
        }

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

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var user = _userRepository.GetByEmail(dto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));
        if (user.PasswordHash != passwordHash)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(new { message = "Login successful", userId = user.Id, username = user.Username });
    }
}

public class RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}