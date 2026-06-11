namespace FinanceTracker.Models.Entities;

public class Category
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }

    public List<Budget> Budgets { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}