using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace paymatesapi.Entities
{

    public class Friend
    {
        public int FriendId { get; set; }
        public required string FriendOneUid { get; set; }

        public required string FriendTwoUid { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}