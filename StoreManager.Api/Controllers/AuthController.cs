using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManager.Api.Data;
using StoreManager.Api.Models;
using StoreManager.Api.Models.Dtos;

namespace StoreManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {

        var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password");
        }

        return Ok(new LoginResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        });
    }

}

