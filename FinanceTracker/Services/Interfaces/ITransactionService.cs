using FinanceTracker.Models.Dtos;

namespace FinanceTracker.Services.Interfaces;

public interface ITransactionService
{
    public Task<List<TransactionDto>?> GetTransactions(int userId, int? accountId, int? categoryId, DateTime? from, DateTime? to,
        decimal? page, decimal? pageSize);

    public Task<TransactionDto?> GetTransaction(int userId, int id);

    public Task<TransactionDto> CreateTransaction(int userId, CreateTransactionDto createTransactionDto);

    public Task<TransactionDto?> UpdateTransaction(int userId, int id, UpdateTransactionDto updateTransactionDto);

    public Task<bool> DeleteTransaction(int userId, int id);
}