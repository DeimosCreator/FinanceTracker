using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    
    public AuthController(IAuthService service) => _service = service;
    [HttpPost]
    public IActionResult Register(string email, string password)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        
    }
}