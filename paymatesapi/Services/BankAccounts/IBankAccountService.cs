using paymatesapi.DTOs;
using paymatesapi.Models;
using Microsoft.AspNetCore.Http;

namespace paymatesapi.Services
{
    public interface IBankAccountService
    {
        BankAccountResponse addBankAccount(BankAccountDto bankAccountDto);
        bool deleteBankAccount(string bankAccountUid);

        List<BankAccountResponse>? getBankAccounts(string userUid);

    }
}
