using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IFriendService
    {
        Task<BaseResponse<Friend>> AddFriend(string username, string friendUsername);

        Task<BaseResponse<bool>> DeleteFriend(string username, string friendUsername);

        BaseResponse<List<UserWithLastTransaction>> GetFriendsWithTransactionsOfUser(
            string username
        );

        BaseResponse<List<string>> GetFriendsOfUser(string username);

        Task<BaseResponse<UserFriendResponse>> FindFriendByUsername(string username);
    }
}
