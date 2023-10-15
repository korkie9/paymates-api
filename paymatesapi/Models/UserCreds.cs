using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Models
{
    public class UserCreds
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
