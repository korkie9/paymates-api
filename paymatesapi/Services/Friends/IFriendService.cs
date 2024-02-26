using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IFriendService
    {
        Task<BaseResponse<string>> AddFriend(string userUid, string friendUid);

        Task<BaseResponse<bool>> DeleteFriend(string userId, string friendUid);

        BaseResponse<List<UserResponse>> GetFriendsOfUser(string userId);

        BaseResponse<List<User>> FindFriendByUsername(string username);

    }
}
