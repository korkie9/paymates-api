using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Models
{
    public class UserCreds
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
