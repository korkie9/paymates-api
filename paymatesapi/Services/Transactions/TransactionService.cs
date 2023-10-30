using paymatesapi.Contexts;
using paymatesapi.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using paymatesapi.DTOs;


namespace paymatesapi.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly DataContext _dataContext;
        public TransactionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Transaction createTransaction(TransactionDTO transactionDTO)
        {
            Guid guid = Guid.NewGuid();
            Transaction newTransaction = new Transaction
            {
                Uid = guid.ToString(),
                Icon = transactionDTO.Icon ?? null,
                Title = transactionDTO.Title,
                Amount = transactionDTO.Amount,
                DebtorUid = transactionDTO.DebtorUid,
                CreditorUid = transactionDTO.CreditorUid,
                DateTime = transactionDTO.DateTime,
            };
            return newTransaction;

        }

        public ICollection<Transaction> getTransactions(string userUid, string friendUid)
        {
            Friend friend = _dataContext.Friends
            .Include(f => f.Transactions)
            .FirstOrDefault(f => (f.FriendOneUid == userUid && f.FriendTwoUid == friendUid) || (f.FriendTwoUid == userUid && f.FriendOneUid == friendUid));
            return friend.Transactions;
        }

    }
}
