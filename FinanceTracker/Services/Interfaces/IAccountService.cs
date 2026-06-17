using FinanceTracker.Models.Dtos;

namespace FinanceTracker.Services.Interfaces;

public interface IAccountService
{
    public Task<List<AccountDto>> GetAccounts(int userId);
    
    public Task<AccountDto?> GetAccount(int userId, int id);
    
    public Task<AccountDto?> CreateAccount(int userId, CreateAccountDto accountDto);

    public Task<AccountDto?> UpdateAccount(int userId, int id, UpdateAccountDto accountDto);

    public Task<bool> DeleteAccount(int userId, int id);
}