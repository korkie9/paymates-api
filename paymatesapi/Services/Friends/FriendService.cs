using Microsoft.EntityFrameworkCore;
using paymatesapi.Contexts;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public class FriendService(DataContext dataContext) : IFriendService
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<string>> AddFriend(string userEmail, string friendEmail)
        {
            bool friendPair_1 = _dataContext.Friends.Any(f =>
                f.FriendOneEmail == userEmail && f.FriendTwoEmail == friendEmail
            );
            if (friendPair_1)
            {
                return new BaseResponse<string>
                {
                    Error = new Error { Message = "Users are already friends" }
                };
            }
            bool friendPair_2 = _dataContext.Friends.Any(f =>
                f.FriendOneEmail == friendEmail && f.FriendTwoEmail == userEmail
            );
            if (friendPair_2)
            {
                return new BaseResponse<string>
                {
                    Error = new Error { Message = "Users are already friends" }
                };
            }
            bool userOneExists = _dataContext.Users.Any(f => f.Email == userEmail);
            bool userTwoExists = _dataContext.Users.Any(f => f.Email == friendEmail);
            if (!userOneExists || !userTwoExists)
            {
                return new BaseResponse<string>
                {
                    Error = new Error { Message = "Users not found" }
                };
            }
            Friend newFriend = new() { FriendOneEmail = userEmail, FriendTwoEmail = friendEmail };
            _dataContext.Add(newFriend);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<string> { Data = "Friend added" };
        }

        public BaseResponse<List<UserResponse>> GetFriendsOfUser(string userId)
        {
            var userFriends = _dataContext
                .Users.FromSqlInterpolated(
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

        public async Task<BaseResponse<bool>> DeleteFriend(string userEmail, string friendEmail)
        {
            var friendPair = _dataContext.Friends.FirstOrDefault(f =>
                (f.FriendOneEmail == userEmail && f.FriendTwoEmail == friendEmail)
                || (f.FriendTwoEmail == userEmail && f.FriendOneEmail == friendEmail)
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
            List<User> users =
            [
                .. _dataContext
                    .Users.Select(user => user)
                    .Where(user => user.Username.Contains(username))
            ];
            return new BaseResponse<List<User>> { Data = users };
        }
    }
}
