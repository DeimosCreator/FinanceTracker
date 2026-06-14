namespace FinanceTracker.Models.Dtos;

public record SummaryDto(decimal TotalIncome, decimal TotalExpense, decimal Balance);

public record CategoryStatDto(string CategoryName, decimal Total, decimal Percentage);