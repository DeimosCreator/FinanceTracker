using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class StatController : BaseController
{
    private readonly IStatService _service;

    public StatController(IStatService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string month)
    {
        if (!DateOnly.TryParseExact($"{month}-01", "yyyy-MM-dd", out var date))
            return BadRequest("Неверный формат месяца, ожидается ГГГГ-ММ");

        var userId = GetUserId();
        var summaries = await _service.GetSummary(userId, date);
        
        return Ok(summaries);
    }

    [HttpGet("by-category")]
    public async Task<IActionResult> GetCategoryStat([FromQuery] string type, [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var userId = GetUserId();
        var categoryStats = await _service.GetCategoryStat(userId, type, from, to);

        if (categoryStats == null)
        {
            return BadRequest("Категория не найдена");
        }

        return Ok(categoryStats);
    }
}