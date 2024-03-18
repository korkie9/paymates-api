using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paymatesapi.DTOs;
using paymatesapi.Helpers;
using paymatesapi.Models;
using paymatesapi.Services;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController(IFriendService friendService, IJwtUtils jwtUtils)
        : ControllerBase
    {
        private readonly IFriendService _friendService = friendService;
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        [HttpPost("add-friend"), Authorize]
        public async Task<ActionResult<BaseResponse<string>>> AddFriend(
            InviteFriendRequest friendEmail
        )
        {
            string userId = _jwtUtils.GetUidFromHeaders();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(
                    new BaseResponse<string>
                    {
                        Error = new Error { Message = "User is not authenticated" }
                    }
                );
            }
            var user = await _friendService.AddFriend(userId, friendEmail.FriendEmail);
            return user?.Error?.Message != null ? BadRequest(user) : Ok(user);
        }

        [HttpDelete("remove-friend"), Authorize]
        public async Task<ActionResult<BaseResponse<string>>> RemoveFriend(FriendDTO creds)
        {
            string userId = _jwtUtils.GetUidFromHeaders();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(
                    new BaseResponse<string>
                    {
                        Error = new Error { Message = "User is not authenticated" }
                    }
                );
            }

            var userIsDeleted = await _friendService.DeleteFriend(userId, creds.FriendUid);
            return userIsDeleted.Data
                ? Ok(userIsDeleted)
                : BadRequest(
                    new BaseResponse<bool>
                    {
                        Error = new Error
                        {
                            Message = "An error occured. Friend could not be deleted."
                        }
                    }
                );
        }

        [HttpGet("get-friends"), Authorize]
        public ActionResult<BaseResponse<List<UserResponse>>> GetUserFriends()
        {
            string userId = _jwtUtils.GetUidFromHeaders();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(
                    new BaseResponse<string>
                    {
                        Error = new Error { Message = "User is not authenticated" }
                    }
                );
            }

            var friends = _friendService.GetFriendsOfUser(userId);
            return friends.Error != null ? Ok(friends) : BadRequest(friends);
        }

        [HttpPost("find-friend"), Authorize]
        public ActionResult<BaseResponse<string>> FindFriend(FindFriendRequest friendEmail)
        {
            var user = _friendService.FindFriendByUsername(friendEmail.FriendEmail);

            return user?.Error?.Message != null ? Ok(user) : BadRequest(user);
        }
    }
}
