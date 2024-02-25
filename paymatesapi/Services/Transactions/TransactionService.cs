using paymatesapi.Contexts;
using paymatesapi.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using paymatesapi.DTOs;
using paymatesapi.Models;


namespace paymatesapi.Services
{
    public class TransactionService(DataContext dataContext) : ITransactionService
    {

        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<Transaction>> CreateTransaction(TransactionDTO transactionDTO)
        {
            var friend = _dataContext.Friends.FirstOrDefault(f =>
                (f.FriendOneUid == transactionDTO.DebtorUid && f.FriendTwoUid == transactionDTO.CreditorUid) ||
                (f.FriendOneUid == transactionDTO.CreditorUid && f.FriendTwoUid == transactionDTO.DebtorUid)
            );
            var createdAt = DateTime.Now;
            if (friend != null)
            {
                Guid guid = Guid.NewGuid();
                Transaction newTransaction = new()
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
                    return new BaseResponse<Transaction> { Error = new Error { Message = "Interal Server Error" } };
                }
                return new BaseResponse<Transaction>{ Data = newTransaction };
            }

            return new BaseResponse<Transaction> { Error = new Error { Message = "Friend pair not found" } };

        }

        public BaseResponse<ICollection<Transaction>> GetTransactions(string userUid, string friendUid)
        {
            var friend = _dataContext.Friends
            .Include(f => f.Transactions)
            .FirstOrDefault(f => (f.FriendOneUid == userUid && f.FriendTwoUid == friendUid) || (f.FriendTwoUid == userUid && f.FriendOneUid == friendUid));
            var res = friend?.Transactions ?? [];
            return new BaseResponse<ICollection<Transaction>> { Data = res};

        }

        public BaseResponse<Transaction>? GetTransaction(string transactionUid)
        {
            var dbTransaction = _dataContext.Transactions.Find(transactionUid);
            if(dbTransaction != null) return new BaseResponse<Transaction> {
                Data = dbTransaction
            };
            return new BaseResponse<Transaction> {
                Error = new Error { Message = "Transaction does not exist" }
            };
        }

        public async Task<BaseResponse<bool>> DeleteTransaction(string transactionUid)
        {
            var transaction = _dataContext.Transactions.Find(transactionUid);
            if (transaction != null)
            {
                _dataContext.Entry(transaction).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
                return new BaseResponse<bool> { Data = true };
            }
            return new BaseResponse<bool> {
                Error = new Error { Message = "Transaction does not exist"}
            };

        }

    }
}
