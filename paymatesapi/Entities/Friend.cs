using System.Text.Json.Serialization;

namespace paymatesapi.Entities
{
    public class Friend
    {
        public int FriendId { get; set; }
        public required string FriendOneEmail { get; set; }

        public required string FriendTwoEmail { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; } = [];
    }
}

