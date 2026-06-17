using FinanceTracker.Models.Dtos;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] string? type)
    {
        var userId = GetUserId();
        var categories = await _service.GetCategories(userId, type);

        if (categories == null)
            return BadRequest("Заданный фильтр не найден");

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        var userId = GetUserId();
        var category = await _service.CreateCategory(userId, createCategoryDto);
        return Ok(category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var userId = GetUserId();
        var categories = await _service.UpdateCategory(userId, id, updateCategoryDto);

        if (categories == null)
            return BadRequest("Категория не найдена");

        return Ok(categories);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var userId = GetUserId();
        var status = await _service.DeleteCategory(userId, id);
        if (!status)
            return BadRequest("Категория не найдена");

        return Ok();
    }
}