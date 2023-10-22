using paymatesapi.Contexts;
using paymatesapi.Models;
using System;

namespace paymatesapi.Services
{
    public class FriendService : IFriendService

    {

        private readonly DataContext _dataContext;
        public FriendService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public string addFriend(string friendId) //TODO: get id from header
        {
            return "added friend";
        }

        public string deleteFriend(string friendId) //TODO: get id from header
        {
            return "deleted friend";
        }
    }
}
