using paymatesapi.Contexts;
using paymatesapi.Entities;
using paymatesapi.DTOs;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public class BankAccountService(DataContext dataContext) : IBankAccountService
    {

        private readonly DataContext _dataContext = dataContext;

        public Task<BaseResponse<BankAccount>> AddBankAccount(BankAccountDto bankAccount)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<BankAccount>> UpdateBankAccount(BankAccountDto bankAccount)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> DeleteBankAccount(string bankAccountId)
        {
            throw new NotImplementedException();
        }

        public BaseResponse<List<BankAccount>> GetBankAccounts(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
