using FinanceTracker.Models.Dtos;

namespace FinanceTracker.Services.Interfaces;

public interface IUserService
{
    public Task CreateUser(CreateUserDto userDto);
}