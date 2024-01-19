using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface ITransactionService
    {
        Task<BaseResponse<Transaction>> createTransaction(TransactionDTO transactionDTO);
        ICollection<Transaction>? getTransactions(string userUid, string friendUid);
        Transaction? getTransaction(string transactionUid);

        Task<bool> deleteTransaction(string transactionUid);


    }
}
