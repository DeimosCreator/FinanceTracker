using FinanceTracker.Data;
using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class StatService : IStatService
{
    private readonly AppDbContext _db;

    public StatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SummaryDto> GetSummary(int userId, DateOnly month)
    {
        var totalIncome = await _db.Transactions
            .AsNoTracking()
            .Where(tr => tr.UserId == userId && 
                         tr.Date.Year == month.Year &&
                         tr.Date.Month == month.Month && 
                         tr.Type == TransactionType.Income)
            .SumAsync(tr => tr.Amount);
        
        var totalExpense = await _db.Transactions
            .AsNoTracking()
            .Where(tr => tr.UserId == userId && 
                         tr.Date.Year == month.Year &&
                         tr.Date.Month == month.Month && 
                         tr.Type == TransactionType.Expense)
            .SumAsync(tr => tr.Amount);

        var balance = totalIncome - totalExpense;

        var summaryDto = new SummaryDto(totalIncome, totalExpense, balance);
        return summaryDto;
    }

    public async Task<List<CategoryStatDto>?> GetCategoryStat(int userId, string type, DateTime? from, DateTime? to)
    {
        if (!Enum.TryParse<CategoryType>(type, out var categoryType))
            return null;

        var sums = await _db.Transactions
            .AsNoTracking()
            .Where(tr => tr.UserId == userId
                         && tr.Category.Type == categoryType
                         && (from == null || tr.Date >= from)
                         && (to == null || tr.Date <= to))
            .GroupBy(tr => tr.Category.Name)
            .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
            .ToListAsync();

        var grandTotal = sums.Sum(s => s.Total);

        var result = sums
            .Select(s => new CategoryStatDto(
                s.Category,
                s.Total,
                grandTotal == 0 ? 0 : s.Total / grandTotal * 100))
            .ToList();

        return result;
    }
}