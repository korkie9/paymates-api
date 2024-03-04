namespace paymatesapi.Models
{
    public class BankAccountResponse
    {
        public required string BankAccountUid { get; set; }
        public required string Bank { get; set; }
        public required string AccountNumber { get; set; }
        public required string NameOnCard { get; set; }
        public required string BranchCode { get; set; } 

    }
}
