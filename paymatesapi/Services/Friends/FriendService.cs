using paymatesapi.Contexts;
using paymatesapi.Models;
using paymatesapi.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;


namespace paymatesapi.Services
{
    public class FriendService(DataContext dataContext) : IFriendService

    {

        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<string>> AddFriend(string userUid, string friendUid) //TODO: check if user is trying to add themselves
        {
            var friendPair_1 = _dataContext.Friends.Any(f => f.FriendOneUid == userUid && f.FriendTwoUid == friendUid);
            if (friendPair_1 != false) return new BaseResponse<string>
            {
                Error = new Error { Message = "Users are already friends" }
            };
            var friendPair_2 = _dataContext.Friends.Any(f => f.FriendOneUid == friendUid && f.FriendTwoUid == userUid);
            if (friendPair_2 != false) return new BaseResponse<string>
            {
                Error = new Error { Message = "Users are already friends" }
            };
            var userOneExists = _dataContext.Users.Any(f => f.Uid == userUid);
            var userTwoExists = _dataContext.Users.Any(f => f.Uid == friendUid);
            if (!userOneExists || !userTwoExists) return new BaseResponse<string>
            {
                Error = new Error { Message = "Users not found" }
            };
            Friend newFriend = new()
            {
                FriendOneUid = userUid,
                FriendTwoUid = friendUid
            };
            _dataContext.Add(newFriend);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<string> { Data = "Friend added" };
        }

        public BaseResponse<List<UserResponse>> GetFriendsOfUser(string userId)
        {
            var userFriends = _dataContext.Users
                .FromSqlInterpolated(
                            $@"SELECT U2.Uid, U2.PhotoUrl, U2.FirstName, U2.LastName, U2.Username
                            FROM Users AS U1
                            INNER JOIN Friends AS F ON U1.Uid = F.FriendOneUid
                            INNER JOIN Users AS U2 ON U2.Uid = F.FriendTwoUid
                            WHERE U1.Uid = '{userId}'"
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
            return new BaseResponse<List<UserResponse>> { Data = userFriends };
        }

        public async Task<BaseResponse<bool>> DeleteFriend(string userId, string friendUid)
        {
            var friendPair = _dataContext.Friends.FirstOrDefault(f =>
                (f.FriendOneUid == userId && f.FriendTwoUid == friendUid) ||
                (f.FriendTwoUid == userId && f.FriendOneUid == friendUid)
            );
            if (friendPair != null)
            {
                _dataContext.Entry(friendPair).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
                return new BaseResponse<bool> { Data = true };
            }

            return new BaseResponse<bool>
            {
                Error = new Error { Message = "Users are not friends" }
            };
        }

        public BaseResponse<List<User>> FindFriendByUsername(string username)
        {
            List<User> users = [
            .. _dataContext
            .Users
            .Select(user => user)
            .Where(user => user.Username.Contains(username))
            ];
            return new BaseResponse<List<User>> { Data = users };
        }

    }
}
