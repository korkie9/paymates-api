using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface ITransactionService
    {
        Task<BaseResponse<Transaction>> CreateTransaction(TransactionDTO transactionDTO);

        BaseResponse<ICollection<Transaction>>? GetTransactions(string userUid, string friendUid);

        BaseResponse<Transaction>? GetTransaction(string transactionUid);

        Task<BaseResponse<bool>> DeleteTransaction(string transactionUid);


    }
}
