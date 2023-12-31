using paymatesapi.DTOs;
using paymatesapi.Entities;

namespace paymatesapi.Services
{
    public interface ITransactionService
    {
        Task<Transaction> createTransaction(TransactionDTO transactionDTO);
        ICollection<Transaction>? getTransactions(string userUid, string friendUid);
        Transaction? getTransaction(string transactionUid);

        Task<bool> deleteTransaction(string transactionUid);


    }
}
