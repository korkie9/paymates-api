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

        // public List<Friend> GetFriends(string userId)
        // {
        //     var friends = _dataContext.Friends
        //         .Where(f => f.FriendOneUid == userId || f.FriendTwoUid == userId)
        //         .ToList();
        //     return friends;
        // }

        public List<UserResponse> GetFriendsOfUser(string userId)
        {
            var userFriends = _dataContext.Users
                .FromSqlInterpolated(
                            $@"SELECT U2.Uid, U2.PhotoUrl, U2.FirstName, U2.LastName, U2.Username
                            FROM Users AS U1
                            INNER JOIN Friends AS F ON U1.Uid = F.FriendOneUid
                            INNER JOIN Users AS U2 ON U2.Uid = F.FriendTwoUid
                            WHERE U1.Uid = {userId}"
                            )
                    .Select(u => new UserResponse
                    {
                        Uid = u.Uid,
                        PhotoUrl = u.PhotoUrl,
                        Username = u.Username,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    })
                .ToList();
            return userFriends;
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
