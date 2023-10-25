using paymatesapi.Contexts;
using paymatesapi.Models;
using paymatesapi.Entities;
using System;
using Microsoft.EntityFrameworkCore;


namespace paymatesapi.Services
{
    public class FriendService : IFriendService

    {

        private readonly DataContext _dataContext;
        public FriendService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<string> addFriend(string userUid, string friendUid) //TODO: check if user is trying to add themselves
        {
            var friendPair_1 = _dataContext.Friends.Any(f => f.FriendOneUid == userUid && f.FriendTwoUid == friendUid);
            if (friendPair_1 != false) return "Users are already friends";
            var friendPair_2 = _dataContext.Friends.Any(f => f.FriendOneUid == friendUid && f.FriendTwoUid == userUid);
            if (friendPair_2 != false) return "Users are already friends";
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
            return "Friend Added";
        }

        public async Task<bool> deleteFriend(string userUid, string friendUid)
        {
            var friendPair = _dataContext.Friends.Find(userUid, friendUid);
            //TODO: modify return to inform client if friendPair doesn't exist
            if (friendPair != null)
            {
                _dataContext.Entry(friendPair).State = EntityState.Deleted;
                _dataContext.SaveChanges();
                return true;
            }
            return false;

        }
    }
}
