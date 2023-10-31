namespace paymatesapi.Model
{
    public class TransactionResponse
    {
        public required string Uid { get; set; }

        public required string? Icon { get; set; }
        public required string Title { get; set; }

        public required decimal Amount { get; set; }

        public required string DebtorUid { get; set; }

        public required string CreditorUid { get; set; }

        public required DateTime CreatedAt { get; set; }

    }

}
