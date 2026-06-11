using FinanceTracker.Data;
using FinanceTracker.Models.Dtos;
using FinanceTracker.Models.Entities;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;
    
    public AccountService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<AccountDto>> GetAccounts(int userId)
    {
        var accounts = await _db.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
        var accountDtos = accounts
            .Select(a => new AccountDto(
                a.Id,
                a.UserId,
                a.Name,
                a.Type,
                a.InitialBalance,
                a.Currency,
                a.CreatedAt))
            .ToList();
        return accountDtos;
    }

    public async Task<AccountDto?> GetAccount(int userId, int id)
    {
        var account = await _db.Accounts.Where(a => a.UserId == userId && a.Id == id).FirstOrDefaultAsync();
        
        if (account == null)
        {
            return null;
        }
        
        var accountDto = new AccountDto(
            account.Id,
            account.UserId,
            account.Name,
            account.Type,
            account.InitialBalance,
            account.Currency,
            account.CreatedAt);
        
        return accountDto;
    }

    public async Task<AccountDto?> CreateAccount(int userId, CreateAccountDto accountDto)
    {
        var account = new Account
        {
            UserId = userId,
            Name = accountDto.Name,
            Type = accountDto.Type,
            InitialBalance = accountDto.InitialBalance,
            Currency = accountDto.Currency,
            CreatedAt = DateTime.UtcNow
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        return new AccountDto(
            account.Id, account.UserId, account.Name, account.Type,
            account.InitialBalance, account.Currency, account.CreatedAt);
    }

    public async Task<AccountDto?> UpdateAccount(int userId, int id, UpdateAccountDto accountDto)
    {
        var account = await _db.Accounts.Where(a => a.UserId == userId && a.Id == id).FirstOrDefaultAsync();
        if (account == null)
        {
            return null;
        }
        
        account.Name = accountDto.Name;
        account.Type = accountDto.Type;
        account.InitialBalance = accountDto.InitialBalance;
        account.Currency = accountDto.Currency;
        await _db.SaveChangesAsync();
        
        return new AccountDto(
            account.Id, account.UserId, account.Name, account.Type,
            account.InitialBalance, account.Currency, account.CreatedAt);
    }

    public async Task<bool> RemoveAccount(int userId, int id)
    {
        var account = await _db.Accounts.Where(a => a.UserId == userId && a.Id == id).FirstOrDefaultAsync();
        if (account == null) return false;

        _db.Accounts.Remove(account);
        await _db.SaveChangesAsync();

        return true;
    }
}