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

        public async Task<Transaction> createTransaction(TransactionDTO transactionDTO)
        {
            Friend friend = _dataContext.Friends.FirstOrDefault(f =>
                (f.FriendOneUid == transactionDTO.DebtorUid && f.FriendTwoUid == transactionDTO.CreditorUid) ||
                (f.FriendOneUid == transactionDTO.CreditorUid && f.FriendTwoUid == transactionDTO.DebtorUid)
            );
            var createdAt = DateTime.Now;
            if (friend != null)
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
                    CreatedAt = createdAt,
                    FriendPair = friend
                };
                try
                {

                    friend.Transactions?.Add(newTransaction);
                    await _dataContext.SaveChangesAsync();
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Returned with error: '{e}'");
                    return null;
                }
                return newTransaction;
            }

            return null;

        }

        public ICollection<Transaction> getTransactions(string userUid, string friendUid)
        {
            var friend = _dataContext.Friends
            .Include(f => f.Transactions)
            .FirstOrDefault(f => (f.FriendOneUid == userUid && f.FriendTwoUid == friendUid) || (f.FriendTwoUid == userUid && f.FriendOneUid == friendUid));
            return friend?.Transactions ?? new List<Transaction>();
        }

        public Transaction? getTransaction(string transactionUid)
        {
            var dbTransaction = _dataContext.Transactions.Find(transactionUid);
            return dbTransaction ?? null;
        }

    }
}
