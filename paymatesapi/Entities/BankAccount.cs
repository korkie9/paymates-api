using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using paymatesapi.Entities;

namespace paymatesapi.Entities
{

    public class BankAccount
    {
        [Key]
        public required string BankAccountUid { get; set; }
        public required string Bank { get; set; }
        public required string AccountNumber { get; set; }
        public required string NameOnCard { get; set; }
        public required string BranchCode { get; set; }
        public string UserUid { get; set; }
        public User User { get; set; }

    }
}