using FinanceTracker.Data;
using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;
using FinanceTracker.Services.Interfaces;

namespace FinanceTracker.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task CreateUser(CreateUserDto userDto)
    {
        var user = new User
        {
            Email = userDto.email,
            PasswordHash = userDto.passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }
}