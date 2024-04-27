using paymatesapi.Entities;

namespace paymatesapi.Models
{
    public class UserWithLastTransaction
    {
        public required UserFriendResponse User { get; set; }
        public Transaction? LastTransaction { get; set; }
    }
}
