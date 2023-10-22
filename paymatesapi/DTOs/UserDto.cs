using System.ComponentModel.DataAnnotations;

namespace paymatesapi.DTOs
{
    public class UserDTO
    {

        public string? Uid { get; set; }
        [EmailAddress(ErrorMessage = "Valid Email is required")]
        public required string Email { get; set; }
        public string? PhotoUrl { get; set; }

        [StringLength(100)]
        public required string Username { get; set; }

        [StringLength(70)]
        public required string FirstName { get; set; }

        [StringLength(70)]
        public required string LastName { get; set; }

        public required string Password { get; set; }

    }
}