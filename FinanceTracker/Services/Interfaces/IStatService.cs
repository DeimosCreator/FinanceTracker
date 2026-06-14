using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;

namespace FinanceTracker.Services.Interfaces;

public interface IStatService
{
    public Task<SummaryDto> GetSummary(int userId, DateOnly month);

    public Task<List<CategoryStatDto>?> GetCategoryStat(int userId, string type, DateTime? from, DateTime? to);
}