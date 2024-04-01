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
            InviteFriendRequest inviteRequest
        )
        {
            var user = await _friendService.AddFriend(
                inviteRequest.Username,
                inviteRequest.FriendUsername
            );
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

        [HttpPost("get-friends"), Authorize]
        public ActionResult<BaseResponse<List<UserWithLastTransaction>>> GetUserFriends(
            GetFriendsRequest req
        )
        {
            var friends = _friendService.GetFriendsOfUser(req.Username);
            return Ok(friends);
        }

        [HttpPost("find-friend"), Authorize]
        public ActionResult<BaseResponse<string>> FindFriend(FindFriendRequest friendUsername)
        {
            var user = _friendService.FindFriendByUsername(friendUsername.FriendUsername);

            return user?.Error?.Message != null ? BadRequest(user) : Ok(user);
        }
    }
}
