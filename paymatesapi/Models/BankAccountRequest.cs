using System.ComponentModel.DataAnnotations;

namespace paymatesapi.Models
{
    public class BankAccountRequest
    {
        public required string BankAccountUid { get; set; }
    }
}
