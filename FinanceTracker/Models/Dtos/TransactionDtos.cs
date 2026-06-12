using FinanceTracker.Models.Entities;

namespace FinanceTracker.Models.Dtos;

public record TransactionDto(int Id, int UserId, int AccountId, int CategoryId, decimal Amount, TransactionType Type, DateTime Date, string Description, DateTime CreatedAt);

public record CreateTransactionDto(int AccountId, int CategoryId, decimal Amount, TransactionType Type, DateTime Date, string Description);

public record UpdateTransactionDto(int AccountId, int CategoryId, decimal Amount, TransactionType Type, DateTime Date, string Description);