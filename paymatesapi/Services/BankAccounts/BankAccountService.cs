using Microsoft.EntityFrameworkCore;
using paymatesapi.Contexts;
using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public class BankAccountService(DataContext dataContext) : IBankAccountService
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<BankAccount>> AddBankAccount(BankAccountDto bankAccount)
        {
            {
                Guid guid = Guid.NewGuid();
                var bankAccountResponse = new BankAccount
                {
                    UserUid = bankAccount.UserUid,
                    BankAccountUid = guid.ToString(),
                    Bank = bankAccount.Bank,
                    AccountNumber = bankAccount.AccountNumber,
                    NameOnCard = bankAccount.NameOnCard,
                    BranchCode = bankAccount.BranchCode,
                    CardType = bankAccount.CardType
                };
                _dataContext.BankAccounts.Add(bankAccountResponse);
                await _dataContext.SaveChangesAsync();

                var res = new BaseResponse<BankAccount> { Data = bankAccountResponse };
                return res;
            }
        }

        public async Task<BaseResponse<bool>> DeleteBankAccount(string bankAccountId)
        {
            var dbBankAccount = _dataContext
                .BankAccounts.Where(acc => acc.BankAccountUid == bankAccountId)
                .FirstOrDefault();
            if (dbBankAccount != null)
            {
                _dataContext.BankAccounts.Remove(dbBankAccount);
                await _dataContext.SaveChangesAsync();
                return new BaseResponse<bool> { Data = true };
            }
            return new BaseResponse<bool>
            {
                Data = false,
                Error = new Error { Message = "Bank Account does not exist" }
            };
        }

        public async Task<BaseResponse<List<BankAccount>>> GetBankAccounts(string userId)
        {
            List<BankAccount> bankAccounts = await _dataContext
                .BankAccounts.Where(b => b.UserUid == userId)
                .ToListAsync();
            return new BaseResponse<List<BankAccount>> { Data = bankAccounts };
        }

        public async Task<BaseResponse<BankAccount>> UpdateBankAccount(BankAccountDto bankAccount)
        {
            if (bankAccount.BankAccountUid == null)
            {
                return new BaseResponse<BankAccount>
                {
                    Error = new Error { Message = "Bank Account is invalid" }
                };
            }
            var oldBankAccount = _dataContext.BankAccounts.FirstOrDefault(e =>
                e.BankAccountUid == bankAccount.BankAccountUid
            );
            if (oldBankAccount != null)
            {
                _dataContext.Entry(oldBankAccount).CurrentValues.SetValues(bankAccount);
                await _dataContext.SaveChangesAsync();
                return new BaseResponse<BankAccount>
                {
                    Data = new BankAccount
                    {
                        UserUid = bankAccount.UserUid,
                        BankAccountUid = bankAccount.BankAccountUid,
                        Bank = bankAccount.Bank,
                        AccountNumber = bankAccount.AccountNumber,
                        NameOnCard = bankAccount.NameOnCard,
                        BranchCode = bankAccount.BranchCode,
                        CardType = bankAccount.CardType
                    }
                };
            }
            return new BaseResponse<BankAccount>
            {
                Error = new Error { Message = "There was an issue updating this bank account" }
            };
        }
    }
}
