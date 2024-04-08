namespace paymatesapi.DTOs
{
    public class TransactionDTO
    {
        public string? Icon { get; set; }
        public required string Title { get; set; }

        public required decimal Amount { get; set; }

        public required string DebtorUsername { get; set; }

        public required string CreditorUsername { get; set; }
    }
}
