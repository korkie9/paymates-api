using paymatesapi.DTOs;
using paymatesapi.Models;
using paymatesapi.Entities;

namespace paymatesapi.Services
{
    public interface IFriendService
    {
        string addFriend(string uid);
        string deleteFriend(string uid);

    }
}
