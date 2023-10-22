using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Entities
{
    [PrimaryKey(nameof(FriendOneUid), nameof(FriendTwoUid))]
    public class Friend
    {
        public required string FriendOneUid { get; set; }

        public required string FriendTwoUid { get; set; }
    }
}