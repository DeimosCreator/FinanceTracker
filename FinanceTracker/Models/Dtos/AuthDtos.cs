namespace FinanceTracker.Models.Dtos;

public record RegisterDto(string Email, string Password);

public record LoginDto(string Email, string Password);