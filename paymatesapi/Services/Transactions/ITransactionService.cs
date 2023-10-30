using paymatesapi.DTOs;
using paymatesapi.Entities;

namespace paymatesapi.Services
{
    public interface ITransactionService
    {
        Transaction createTransaction(TransactionDTO transactionDTO);
        ICollection<Transaction> getTransactions(string userUid, string friendUid);

    }
}
