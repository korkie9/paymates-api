namespace paymatesapi.DTOs
{
    public class BankAccountDto
    {
        public string? BankAccountUid { get; set; }

        public required string Bank { get; set; }

        public required string AccountNumber { get; set; }

        public required string NameOnCard { get; set; }

        public required string BranchCode { get; set; }

        public required string UserUid { get; set; }

        public required string CardType { get; set; }
    }
}

