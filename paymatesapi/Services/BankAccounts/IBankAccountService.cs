using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IBankAccountService
    {
        Task<BaseResponse<BankAccount>> AddBankAccount(BankAccountDto bankAccount);

        Task<BaseResponse<BankAccount>> UpdateBankAccount(BankAccountDto bankAccount);

        Task<BaseResponse<bool>> DeleteBankAccount(string bankAccountId);

        Task<BaseResponse<List<BankAccount>>> GetBankAccounts(string userId);
    }
}
