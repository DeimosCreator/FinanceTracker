using FinanceTracker.Models.Dtos;

namespace FinanceTracker.Services.Interfaces;

public interface ICategoryService
{
    public Task<List<CategoryDto>?> GetCategories(int userId, string? filter);

    public Task<CategoryDto> CreateCategory(int userId, CreateCategoryDto createCategoryDto);

    public Task<CategoryDto?> UpdateCategory(int userId, int id, UpdateCategoryDto updateCategoryDto);

    public Task<bool> RemoveCategory(int userId, int id);
}