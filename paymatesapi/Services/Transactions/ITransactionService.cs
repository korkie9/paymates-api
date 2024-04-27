using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface ITransactionService
    {
        Task<BaseResponse<bool>> CreateTransactions(TransactionDTO transactionDTO);

        BaseResponse<ICollection<Transaction>> GetTransactions(
            string username,
            string friendUsername
        );

        BaseResponse<Transaction>? GetTransaction(string transactionUid);

        Task<BaseResponse<bool>> DeleteTransaction(string transactionUid);
    }
}
