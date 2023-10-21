using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Models
{
    public class RefreshTokenRequest
    {
        public required string Uid { get; set; }
        public required string RefreshToken { get; set; }
    }
}
