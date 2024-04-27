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

        private async Task<BaseResponse<Transaction>> CreateTransaction(
            string DebtorUsername,
            string CreditorUsername,
            string Icon,
            string Title,
            decimal Amount
        )
        {
            Friend? friend = _dataContext.Friends.FirstOrDefault(f =>
                (f.FriendOneUsername == DebtorUsername && f.FriendTwoUsername == CreditorUsername)
                || (
                    f.FriendOneUsername == CreditorUsername && f.FriendTwoUsername == DebtorUsername
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
                        Icon = Icon ?? null,
                        Title = Title,
                        Amount = Amount,
                        DebtorUsername = DebtorUsername,
                        CreditorUsername = CreditorUsername,
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

        private async Task<BaseResponse<bool>> CreateMultipleTransactions(
            int length,
            TransactionDTO transactionDTO,
            string debtorOrCreditor
        )
        {
            for (int i = 0; i < length; i++)
            {
                var res = await CreateTransaction(
                    transactionDTO.DebtorUsernames[debtorOrCreditor == "debtor" ? i : 0],
                    transactionDTO.CreditorUsernames[debtorOrCreditor == "creditor" ? i : 0],
                    transactionDTO.Icon,
                    transactionDTO.Title,
                    transactionDTO.Amount / length
                );
                if (res.Error?.Message != null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Error = new Error { Message = res.Error?.Message }
                    };
                }
            }
            return new BaseResponse<bool> { Data = true, };
        }

        public async Task<BaseResponse<bool>> CreateTransactions(TransactionDTO transactionDTO)
        {
            int lengthOfDebtors = transactionDTO.DebtorUsernames.Length;
            int lengthOfCreditors = transactionDTO.CreditorUsernames.Length;
            if (lengthOfDebtors < 1 || lengthOfCreditors < 1)
            {
                return new BaseResponse<bool>
                {
                    Error = new Error { Message = "One of more debtor and creditor is required" }
                };
            }
            if (lengthOfDebtors > 1 && lengthOfCreditors > 1)
            {
                return new BaseResponse<bool>
                {
                    Error = new Error
                    {
                        Message = "Either debtor or creditor must have an array length of one"
                    }
                };
            }
            if (lengthOfDebtors == 1 && lengthOfCreditors == 1)
            {
                return await CreateMultipleTransactions(1, transactionDTO, "debtor");
            }
            if (lengthOfDebtors > 1)
            {
                return await CreateMultipleTransactions(lengthOfDebtors, transactionDTO, "debtor");
            }

            return lengthOfCreditors > 1
                ? await CreateMultipleTransactions(lengthOfCreditors, transactionDTO, "creditor")
                : new BaseResponse<bool> { Data = true };
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
            var res =
                friend?.Transactions.OrderBy(t => t.CreatedAt).ToList() ?? new List<Transaction>();
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
