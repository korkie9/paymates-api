using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Models
{
    public class User : UserModel
    {
        [Key]
        public required string Uid { get; set; }
    }
}