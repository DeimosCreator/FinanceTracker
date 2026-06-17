using FinanceTracker.Data;
using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryDto>?> GetCategories(int userId, string? filter)
    {
        if (!Enum.TryParse<CategoryType>(filter, out var categoryType) && !string.IsNullOrEmpty(filter))
            return null;
        
        var categoriesDto = await _db.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId && (c.Type == categoryType || string.IsNullOrEmpty(filter)))
            .Select(c => new CategoryDto(c.Id, c.UserId, c.Name, c.Type))
            .ToListAsync();

        return categoriesDto;
    }

    public async Task<CategoryDto> CreateCategory(int userId, CreateCategoryDto createCategoryDto)
    {
        var category = new Category
        {
            UserId = userId,
            Name = createCategoryDto.Name,
            Type = createCategoryDto.Type
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        
        var categoryDto = new CategoryDto(
            category.Id, category.UserId, 
            category.Name, category.Type);

        return categoryDto;
    }

    public async Task<CategoryDto?> UpdateCategory(int userId, int id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _db.Categories.Where(c => c.UserId == userId && c.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            return null;
        }

        category.Name = updateCategoryDto.Name;
        category.Type = updateCategoryDto.Type;

        await _db.SaveChangesAsync();

        var categoryDto = new CategoryDto(
            category.Id, category.UserId, category.Name, category.Type);
        
        return categoryDto;
    }

    public async Task<bool> DeleteCategory(int userId, int id)
    {
        var category = await _db.Categories.Where(c => c.UserId == userId && c.Id == id).FirstOrDefaultAsync();
        if (category == null) return false;
        
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();

        return true;
    }
}