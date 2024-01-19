using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace paymatesapi.Entities
{
    public class User
    {
        [Key]
        public required string Uid { get; set; }

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

        public required string RefreshToken { get; set; }

        public required long RefreshTokenExpiry { get; set; }

        [JsonIgnore]
        public ICollection<BankAccount> BankAccounts { get; } = new List<BankAccount>();

    }
}