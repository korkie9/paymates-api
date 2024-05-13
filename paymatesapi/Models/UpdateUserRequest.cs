namespace paymatesapi.Models
{
    public class UserUpdateRequest
    {
        public required string Uid { get; set; }

        public string? PhotoUrl { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    }
}
