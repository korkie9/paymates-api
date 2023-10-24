using paymatesapi.Contexts;
using paymatesapi.Models;
using paymatesapi.Entities;
using System;

namespace paymatesapi.Services
{
    public class FriendService : IFriendService

    {

        private readonly DataContext _dataContext;
        public FriendService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<string> addFriend(string userUid, string friendUid) //TODO: get id from header
        {
            var friendPair_1 = _dataContext.Friends.Any(f => f.FriendOneUid == userUid && f.FriendTwoUid == friendUid);
            if (friendPair_1 != false) return null;
            var friendPair_2 = _dataContext.Friends.Any(f => f.FriendOneUid == friendUid && f.FriendTwoUid == userUid);
            if (friendPair_2 != false) return null;
            var userOneExists = _dataContext.Users.Any(f => f.Uid == userUid);
            var userTwoExists = _dataContext.Users.Any(f => f.Uid == friendUid);
            if (!userOneExists || !userTwoExists) return "User not found";
            Friend newFriend = new Friend
            {
                FriendOneUid = userUid,
                FriendTwoUid = friendUid
            };
            _dataContext.Add(newFriend);
            await _dataContext.SaveChangesAsync();
            return "added friend";
        }

        public string deleteFriend(string friendId) //TODO: get id from header
        {
            return "deleted friend";
        }
    }
}
