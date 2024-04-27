using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Entities
{

    public class BankAccount
    {
        [Key]
        public required string BankAccountUid { get; set; }
        public required string Bank { get; set; }
        public required string AccountNumber { get; set; }
        public required string NameOnCard { get; set; }
        public required string BranchCode { get; set; }
        public required string UserUid { get; set; }
        public User? User { get; set; }

    }
}