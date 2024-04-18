using Microsoft.EntityFrameworkCore;
using paymatesapi.Contexts;
using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public class TransactionService(DataContext dataContext) : ITransactionService
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<Transaction>> CreateTransaction(
            TransactionDTO transactionDTO
        )
        {
            Friend? friend = _dataContext.Friends.FirstOrDefault(f =>
                (
                    f.FriendOneUsername == transactionDTO.DebtorUsername
                    && f.FriendTwoUsername == transactionDTO.CreditorUsername
                )
                || (
                    f.FriendOneUsername == transactionDTO.CreditorUsername
                    && f.FriendTwoUsername == transactionDTO.CreditorUsername
                )
            );
            long createdAt = DateTime.Now.ToFileTimeUtc();
            if (friend != null)
            {
                Guid guid = Guid.NewGuid();
                Transaction newTransaction =
                    new()
                    {
                        Uid = guid.ToString(),
                        Icon = transactionDTO.Icon ?? null,
                        Title = transactionDTO.Title,
                        Amount = transactionDTO.Amount,
                        DebtorUsername = transactionDTO.DebtorUsername,
                        CreditorUsername = transactionDTO.CreditorUsername,
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
                    return new BaseResponse<Transaction>
                    {
                        Error = new Error { Message = "Interal Server Error" }
                    };
                }
                return new BaseResponse<Transaction> { Data = newTransaction };
            }

            return new BaseResponse<Transaction>
            {
                Error = new Error { Message = "Friend pair not found" }
            };
        }

        public BaseResponse<ICollection<Transaction>> GetTransactions(
            string username,
            string friendUsername
        )
        {
            var friend = _dataContext
                .Friends.Include(f => f.Transactions)
                .FirstOrDefault(f =>
                    (f.FriendOneUsername == username && f.FriendTwoUsername == friendUsername)
                    || (f.FriendTwoUsername == username && f.FriendOneUsername == friendUsername)
                );
            var res = friend?.Transactions ?? [];
            Console.WriteLine(res.ToString());
            return new BaseResponse<ICollection<Transaction>> { Data = res };
        }

        public BaseResponse<Transaction>? GetTransaction(string transactionUid)
        {
            Transaction? dbTransaction = _dataContext.Transactions.Find(transactionUid);
            return dbTransaction != null
                ? new BaseResponse<Transaction> { Data = dbTransaction }
                : new BaseResponse<Transaction>
                {
                    Error = new Error { Message = "Transaction does not exist" }
                };
        }

        public async Task<BaseResponse<bool>> DeleteTransaction(string transactionUid)
        {
            Transaction? transaction = _dataContext.Transactions.Find(transactionUid);
            if (transaction != null)
            {
                _dataContext.Entry(transaction).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
                return new BaseResponse<bool> { Data = true };
            }
            return new BaseResponse<bool>
            {
                Error = new Error { Message = "Transaction does not exist" }
            };
        }
    }
}
