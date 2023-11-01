using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using paymatesapi.Entities;

namespace paymatesapi.Entities
{
    public class Transaction
    {
        [Key]
        public required string Uid { get; set; }

        public required string? Icon { get; set; }
        public required string Title { get; set; }

        public required decimal Amount { get; set; }

        public required string DebtorUid { get; set; }

        public required string CreditorUid { get; set; }

        public required DateTime CreatedAt { get; set; }

        public int FriendId { get; set; }
        public Friend FriendPair { get; set; }

    }

}