using FinanceTracker.Models.Dtos;

namespace FinanceTracker.Services.Interfaces;

public interface IAuthService
{
    public void Register(CreateRegisterDto registerDto);
}