using FinanceTracker.Models.Dtos;

namespace FinanceTracker.Services.Interfaces;

public interface IAuthService
{
    public Task Register(RegisterDto registerDto);

    public Task<string?> Login(LoginDto dto);
}