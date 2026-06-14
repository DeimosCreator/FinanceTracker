using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceTracker.Data;
using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(IUserService userService, AppDbContext db, IConfiguration config)
    {
        _userService = userService;
        _db = db;
        _config = config;
    }
    
    public async Task Register(RegisterDto registerDto)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);  // хешируем здесь
        var userDto = new CreateUserDto(registerDto.Email, hash);
        await _userService.CreateUser(userDto);
    }
    
    public async Task<string?> Login(LoginDto dto)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}