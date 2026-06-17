using FinanceTracker.Models.Dtos;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionController : BaseController
{
    private readonly ITransactionService _service;

    public TransactionController(ITransactionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] int? accountId, [FromQuery] int? categoryId,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] decimal? page, [FromQuery] decimal? pageSize)
    {
        var userId = GetUserId();
        var transactions = await _service.GetTransactions(userId, accountId, categoryId, from, to, page, pageSize);

        if (transactions == null)
            return BadRequest();

        return Ok(transactions);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var userId = GetUserId();
        var transaction = await _service.GetTransaction(userId, id);

        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto createTransactionDto)
    {
        var userId = GetUserId();
        var transaction = await _service.CreateTransaction(userId, createTransactionDto);
        
        if (transaction == null)
            return BadRequest();

        return Ok(transaction);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionDto updateTransactionDto)
    {
        var userId = GetUserId();
        var transaction = await _service.UpdateTransaction(userId, id, updateTransactionDto);

        if (transaction == null)
            return BadRequest("Не удалось найти транзакцию");

        return Ok(transaction);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var userId = GetUserId();
        var status = await _service.DeleteTransaction(userId, id);

        return status ? Ok() : BadRequest("Транзакция не найдена");
    }
}