namespace paymatesapi.Models
{
    public class GetTransactionsRequest
    {
        public required string FriendUsername { get; set; }
        public required string Username { get; set; }
    }
}
