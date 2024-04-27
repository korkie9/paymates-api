using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using paymatesapi.Models;

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

        public string? RefreshToken { get; set; }

        public long? RefreshTokenExpiry { get; set; }

        public bool? Verified { get; set; }

        [JsonIgnore]
        public ICollection<BankAccount> BankAccounts { get; } = [];

        public static implicit operator User(BaseResponse<User> v)
        {
            throw new NotImplementedException();
        }
    }
}

