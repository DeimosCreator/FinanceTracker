using FinanceTracker.Models.Dtos;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseController
{
    private IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        var userId = GetUserId();
        var accounts = await _service.GetAccounts(userId);
        
        if (accounts.Count == 0)
            return Ok("Аккаунты не найдены");

        return Ok(accounts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var userId = GetUserId();
        var account = await _service.GetAccount(userId, id);
        if (account == null)
        {
            return NotFound("Счёт не найден");
        }

        return Ok(account);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto accountDto)
    {
        var userId = GetUserId();
        var account = await _service.CreateAccount(userId, accountDto);
        if (account is null)
        {
            return BadRequest("Не удалось создать счёт"); 
        }

        return CreatedAtAction(
            nameof(GetAccount),
            new { id = account.Id },
            account);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDto accountDto, int id)
    {
        var userId = GetUserId();
        var account = await _service.UpdateAccount(userId, id, accountDto);
        if (account is null)
        {
            return BadRequest("Не удалось обновить счёт"); 
        }

        return Ok(account);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var userId = GetUserId();
        var status = await _service.DeleteAccount(userId, id);

        return status ? Ok() : BadRequest("Не удалось удалить счёт");
    }
}