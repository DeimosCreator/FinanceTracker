using FinanceTracker.Models.Dtos;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _service;
    
    public AuthController(IAuthService service) => _service = service;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        await _service.Register(dto);
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _service.Login(dto);
        if (token is null)
            return Unauthorized("Неверный email или пароль");

        return Ok(new { token });
    }
}