using paymatesapi.Contexts;
using paymatesapi.Models;
using paymatesapi.Entities;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;


namespace paymatesapi.Services
{
    public class BankAccountService : IBankAccountService

    {

        private readonly DataContext _dataContext;
        public BankAccountService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public BankAccountResponse addBankAccount(BankAccountDto bankAccountDto) //TODO: get id from param and dto from body anf add account
        {
            Guid guid = Guid.NewGuid();
            var bankAccountResponse = new BankAccountResponse
            {

                BankAccountUid = guid.ToString(),
                Bank = bankAccountDto.Bank,
                AccountNumber = bankAccountDto.AccountNumber,
                NameOnCard = bankAccountDto.NameOnCard,
                BranchCode = bankAccountDto.BranchCode
            };
            // friend.Transactions?.Add(newTransaction);

            return bankAccountResponse;
        }

        public List<BankAccountResponse>? getBankAccounts(string userUid)
        {
            return null;
        }

        public bool deleteBankAccount(string bankAccountUid)
        {
            return true;
        }
    }
}
