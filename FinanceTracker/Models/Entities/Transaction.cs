namespace FinanceTracker.Models.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AccountId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}