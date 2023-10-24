using paymatesapi.DTOs;
using paymatesapi.Models;
using paymatesapi.Entities;

namespace paymatesapi.Services
{
    public interface IFriendService
    {
        Task<string> addFriend(string userUid, string friendUid);
        string deleteFriend(string uid);

    }
}
