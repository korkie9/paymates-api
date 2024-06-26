using Microsoft.EntityFrameworkCore;
using paymatesapi.Contexts;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public class FriendService(DataContext dataContext) : IFriendService
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<Friend>> AddFriend(string username, string friendUsername)
        {
            bool friendPair_1 = await _dataContext.Friends.AnyAsync(f =>
                f.FriendOneUsername == username && f.FriendTwoUsername == friendUsername
            );
            if (friendPair_1)
            {
                return new BaseResponse<Friend>
                {
                    Error = new Error { Message = "Users are already friends" }
                };
            }
            bool friendPair_2 = await _dataContext.Friends.AnyAsync(f =>
                f.FriendOneUsername == friendUsername && f.FriendTwoUsername == username
            );
            if (friendPair_2)
            {
                return new BaseResponse<Friend>
                {
                    Error = new Error { Message = "Users are already friends" }
                };
            }
            var userOne = await _dataContext.Users.FirstOrDefaultAsync(f => f.Username == username);
            var userTwo = await _dataContext.Users.FirstOrDefaultAsync(f =>
                f.Username == friendUsername
            );
            if (userOne == null || userTwo == null)
            {
                return new BaseResponse<Friend>
                {
                    Error = new Error { Message = "Users not found" }
                };
            }
            Friend newFriend =
                new() { FriendOneUsername = username, FriendTwoUsername = friendUsername };
            _dataContext.Add(newFriend);
            await _dataContext.SaveChangesAsync();
            return new BaseResponse<Friend> { Data = newFriend };
        }

        public BaseResponse<List<UserWithLastTransaction>> GetFriendsWithTransactionsOfUser(
            string username
        )
        {
            var friendUsernames = _dataContext
                .Friends.Where(f =>
                    f.FriendOneUsername == username || f.FriendTwoUsername == username
                )
                .Select(f =>
                    f.FriendOneUsername == username ? f.FriendTwoUsername : f.FriendOneUsername
                )
                .Distinct()
                .ToList();

            var friendsData = _dataContext
                .Users.Where(u => friendUsernames.Contains(u.Username))
                .ToList();

            var userTransactions = _dataContext
                .Transactions.Where(t =>
                    t.CreditorUsername == username || t.DebtorUsername == username
                )
                .GroupBy(t =>
                    t.CreditorUsername == username ? t.DebtorUsername : t.CreditorUsername
                )
                .Select(g => new
                {
                    Username = g.Key,
                    LastTransaction = g.OrderByDescending(t => t.CreatedAt).FirstOrDefault()
                })
                .ToList();

            var result = friendsData
                .Select(u => new UserWithLastTransaction
                {
                    User = new UserFriendResponse
                    {
                        Email = u.Email,
                        PhotoUrl = u.PhotoUrl,
                        Username = u.Username,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    },
                    LastTransaction = userTransactions
                        .FirstOrDefault(t => t.Username == u.Username)
                        ?.LastTransaction
                })
                .ToList();

            return new BaseResponse<List<UserWithLastTransaction>> { Data = result };
        }

        public BaseResponse<List<string>> GetFriendsOfUser(string username)
        {
            var friends = _dataContext
                .Friends.Where(f =>
                    f.FriendOneUsername == username || f.FriendTwoUsername == username
                )
                .ToList()
                .Select(f =>
                {
                    return f.FriendOneUsername == username
                        ? f.FriendOneUsername
                        : f.FriendTwoUsername;
                })
                .ToList();

            return new BaseResponse<List<string>> { Data = friends };
        }

        public async Task<BaseResponse<bool>> DeleteFriend(string username, string friendUsername)
        {
            var friendPair = _dataContext.Friends.FirstOrDefault(f =>
                (f.FriendOneUsername == username && f.FriendTwoUsername == friendUsername)
                || (f.FriendTwoUsername == username && f.FriendOneUsername == friendUsername)
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

        public async Task<BaseResponse<UserFriendResponse>> FindFriendByUsername(string username)
        {
            var user = await _dataContext
                .Users.Where(user => user.Username.Contains(username))
                .FirstOrDefaultAsync();
            if (user != null)
            {
                UserFriendResponse userFriendResponse = new UserFriendResponse
                {
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return new BaseResponse<UserFriendResponse> { Data = userFriendResponse };
            }
            return new BaseResponse<UserFriendResponse>
            {
                Error = new Error { Message = "User not found" }
            };
        }
    }
}
