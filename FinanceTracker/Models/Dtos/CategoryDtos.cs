using FinanceTracker.Models.Entities;

namespace FinanceTracker.Models.Dtos;

public record CategoryDto(int Id, int UserId, string Name, CategoryType Type);

public record CreateCategoryDto(string Name, CategoryType Type);

public record UpdateCategoryDto(string Name, CategoryType Type);