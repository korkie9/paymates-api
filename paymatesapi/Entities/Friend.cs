using System.Text.Json.Serialization;

namespace paymatesapi.Entities
{
    public class Friend
    {
        public int FriendId { get; set; }

        public required string FriendOneUsername { get; set; }

        public required string FriendTwoUsername { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; } = [];
    }
}
