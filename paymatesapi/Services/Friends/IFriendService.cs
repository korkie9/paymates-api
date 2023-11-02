using paymatesapi.DTOs;
using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IFriendService
    {
        Task<string> addFriend(string userUid, string friendUid);
        Task<bool> deleteFriend(string userId, string friendUid);

        List<UserResponse> GetFriendsOfUser(string userId);

    }
}
