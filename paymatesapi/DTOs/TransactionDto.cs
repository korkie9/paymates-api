using System.ComponentModel.DataAnnotations;
using paymatesapi.Entities;
namespace paymatesapi.DTOs
{
    public class TransactionDTO
    {
        public string? Icon { get; set; }
        public required string Title { get; set; }

        public required decimal Amount { get; set; }

        public required string DebtorUid { get; set; }

        public required string CreditorUid { get; set; }

        public required DateTime DateTime { get; set; }

    }
}