using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Entities
{
    [PrimaryKey(nameof(Uid), nameof(FriendUid))]
    public class Friend
    {
        public required string Uid { get; set; }

        public required string FriendUid { get; set; }
    }
}