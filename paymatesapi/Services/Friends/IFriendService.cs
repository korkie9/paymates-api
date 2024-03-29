using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IFriendService
    {
        Task<BaseResponse<string>> AddFriend(string userEmail, string friendEmail);

        Task<BaseResponse<bool>> DeleteFriend(string userEmail, string friendEmail);

        BaseResponse<List<UserResponse>> GetFriendsOfUser(string userId);

        BaseResponse<List<User>> FindFriendByUsername(string username);
    }
}
