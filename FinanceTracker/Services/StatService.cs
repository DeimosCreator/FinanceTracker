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
            .Where(tr => tr.UserId == userId && tr.Date.Month == month.Month && tr.Type == TransactionType.Income)
            .SumAsync(tr => tr.Amount);
        
        var totalExpense = await _db.Transactions
            .AsNoTracking()
            .Where(tr => tr.UserId == userId && tr.Date.Month == month.Month && tr.Type == TransactionType.Expense)
            .SumAsync(tr => tr.Amount);

        var initialBalance = await _db.Accounts
            .AsNoTracking()
            .Where(a => a.UserId == userId &&
                        _db.Transactions
                            .Any(tr => tr.UserId == userId &&
                                       tr.Date.Month == month.Month &&
                                       tr.AccountId == a.Id))
            .SumAsync(a => a.InitialBalance);
        
        var balance =  totalIncome - totalExpense + initialBalance;

        var summaryDto = new SummaryDto(totalIncome, totalExpense, balance);
        return summaryDto;
    }

    public async Task<List<CategoryStatDto>?> GetCategoryStat(int userId, string type, DateTime? from, DateTime? to)
    {
        if (!Enum.TryParse<CategoryType>(type, out var categoryType))
            return null;
        
        var categoryStatDtos = await _db.Categories
            .AsNoTracking()
            .Where(cat => cat.UserId == userId && cat.Type == categoryType)
            .Include(cat => cat.Transactions)
            .Select(cat => new CategoryStatDto(
                cat.Name, 
                cat.Transactions
                    .Where(tr => (from == null || tr.Date > from) &&
                                 (to == null || tr.Date <= to))
                    .Sum(tr => tr.Amount),
                cat.Transactions
                    .Where(tr => (from == null || tr.Date > from) &&
                                 (to == null || tr.Date <= to))
                    .Sum(tr => tr.Amount) / _db.Transactions
                    .Where(tr => (from == null || tr.Date > from) &&
                                 (to == null || tr.Date <= to))
                    .Sum(tr => tr.Amount) * 100))
            .ToListAsync();

        return categoryStatDtos;
    }
}