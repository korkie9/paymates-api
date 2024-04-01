namespace paymatesapi.Models
{
    public class UserFriendResponse
    {
        public required string Email { get; set; }

        public string? PhotoUrl { get; set; }

        public required string Username { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    }
}
