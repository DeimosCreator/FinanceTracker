namespace FinanceTracker.Models.Entities;

public class Budget
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal LimitAmount { get; set; }
    public DateOnly Month { get; set; }
}