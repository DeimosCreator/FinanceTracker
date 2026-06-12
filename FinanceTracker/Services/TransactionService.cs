using FinanceTracker.Data;
using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;

    public TransactionService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<TransactionDto>?> GetTransactions(int userId, int? accountId, int? categoryId, DateTime? from, DateTime? to, decimal? page, decimal? pageSize)
    {
        if ((page == null && pageSize != null) || (page != null && pageSize == null))
            return null;
        if (page == null || pageSize == null)
        {
            page = 0;
            pageSize = int.MaxValue;
        }

        var transactionsDto = await _db.Transactions
            .Where(tr => tr.UserId == userId &&
                         (accountId == null || tr.AccountId == accountId) &&
                         (categoryId == null || tr.CategoryId == categoryId) &&
                         (from == null || tr.CreatedAt >= from) &&
                         (to == null || tr.CreatedAt <= to))
            .Skip((int)(page - 1))
            .Take((int)pageSize)
            .Select(tr => new TransactionDto(tr.Id, tr.UserId, tr.AccountId,
                tr.CategoryId, tr.Amount, tr.Type, tr.Date, tr.Description, tr.CreatedAt))
            .ToListAsync();
        
        return transactionsDto;
    }

    public async Task<TransactionDto?> GetTransaction(int userId, int id)
    {
        var transaction = await _db.Transactions
            .Where(tr => tr.UserId == userId && tr.Id == id)
            .FirstOrDefaultAsync();

        if (transaction == null)
            return null;

        var transactionDto = new TransactionDto(transaction.Id, transaction.UserId, transaction.AccountId,
            transaction.CategoryId, transaction.Amount, transaction.Type, transaction.Date, transaction.Description,
            transaction.CreatedAt);

        return transactionDto;
    }

    public async Task<TransactionDto> CreateTransaction(int userId, CreateTransactionDto createTransactionDto)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            AccountId = createTransactionDto.AccountId,
            CategoryId = createTransactionDto.CategoryId,
            Amount = createTransactionDto.Amount,
            Type = createTransactionDto.Type,
            Date = createTransactionDto.Date,
            Description = createTransactionDto.Description,
            CreatedAt = DateTime.UtcNow
        };
        
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        var transactionDto = new TransactionDto(transaction.Id, transaction.UserId, transaction.AccountId,
            transaction.CategoryId, transaction.Amount, transaction.Type, transaction.Date, transaction.Description,
            transaction.CreatedAt);

        return transactionDto;
    }

    public async Task<TransactionDto?> UpdateTransaction(int userId, int id, UpdateTransactionDto updateTransactionDto)
    {
        var transaction = await _db.Transactions
            .Where(tr => tr.UserId == userId && tr.Id == id)
            .FirstOrDefaultAsync();

        if (transaction == null) 
            return null;

        transaction.AccountId = updateTransactionDto.AccountId;
        transaction.CategoryId = updateTransactionDto.CategoryId;
        transaction.Amount = updateTransactionDto.Amount;
        transaction.Type = updateTransactionDto.Type;
        transaction.Date = updateTransactionDto.Date;
        transaction.Description = updateTransactionDto.Description;
        
        await _db.SaveChangesAsync();
        
        var transactionDto = new TransactionDto(transaction.Id, transaction.UserId, transaction.AccountId,
            transaction.CategoryId, transaction.Amount, transaction.Type, transaction.Date, transaction.Description,
            transaction.CreatedAt);

        return transactionDto;
    }

    public async Task<bool> DeleteTransaction(int userId, int id)
    {
        var transaction = await _db.Transactions
            .Where(tr => tr.UserId == userId && tr.Id == id)
            .FirstOrDefaultAsync();

        if (transaction == null)
            return false;

        _db.Transactions.Remove(transaction);
        await _db.SaveChangesAsync();

        return true;
    }
}