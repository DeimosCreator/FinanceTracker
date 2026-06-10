using FinanceTracker.Models.Dtos;
using FinanceTracker.Services.Interfaces;

namespace FinanceTracker.Services;

public class AuthService : IAuthService
{
    private readonly UserService _userService;

    public AuthService(UserService userService)
    {
        _userService = userService;
    }
    
    public async void Register(CreateRegisterDto registerDto)
    {
        var userDto = new CreateUserDto
        (
            registerDto.email,
            registerDto.passworHash
        );

        await _userService.CreateUser(userDto);
    }
}