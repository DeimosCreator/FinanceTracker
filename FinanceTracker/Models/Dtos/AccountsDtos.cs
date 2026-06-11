using FinanceTracker.Models.Entities;

namespace FinanceTracker.Models.Dtos;

public record AccountDto(int Id, int UserId, string Name, AccountType Type, decimal InitialBalance, string Currency, DateTime CreatedAt);

public record CreateAccountDto(string Name, AccountType Type, decimal InitialBalance, string Currency);

public record UpdateAccountDto(string Name, AccountType Type, decimal InitialBalance, string Currency);