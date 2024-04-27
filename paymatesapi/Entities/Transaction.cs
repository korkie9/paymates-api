using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Entities
{
    public class Transaction
    {
        [Key]
        public required string Uid { get; set; }

        public string? Icon { get; set; }

        public required string Title { get; set; }

        public required decimal Amount { get; set; }

        public required string DebtorUsername { get; set; }

        public required string CreditorUsername { get; set; }

        public required long CreatedAt { get; set; }

        public int FriendId { get; set; }

        public Friend? FriendPair { get; set; }
    }
}
