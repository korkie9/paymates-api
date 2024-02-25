using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using paymatesapi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using paymatesapi.Helpers;
using paymatesapi.Models;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController(IFriendService friendService, IJwtUtils jwtUtils) : ControllerBase
    {
        private readonly IFriendService _friendService = friendService;
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        [HttpPost("add-friend"), Authorize]
        public async Task<ActionResult<BaseResponse<string>>> AddFriend(FriendDTO request)
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse<string> { 
                Error = new Error {
                    Message = "User is not authenticated"
                }
            });

            var user = await _friendService.AddFriend(userId, request.FriendUid);
            if (user?.Error?.Message != null) return BadRequest(user);
            return Ok(user);
        }

        [HttpDelete("remove-friend"), Authorize]
        public async Task<ActionResult<BaseResponse<string>>> RemoveFriend(FriendDTO creds)
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse<string> { 
                Error = new Error {
                    Message = "User is not authenticated"
                }
            });

            var userIsDeleted = await _friendService.DeleteFriend(userId, creds.FriendUid);
            if (userIsDeleted.Data) return Ok(userIsDeleted);
            return BadRequest(new BaseResponse<bool> { 
                Error = new Error { 
                    Message = "An error occured. Friend could not be deleted."
                }
            });
        }

        [HttpGet("get-friends"), Authorize]
        public ActionResult<BaseResponse<List<UserResponse>>> GetUserFriends()
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse<string> { 
                Error = new Error {
                    Message = "User is not authenticated"
                }
            });

            var friends = _friendService.GetFriendsOfUser(userId);
            if (friends.Error != null) return BadRequest(friends);
            return Ok(friends);
        }

    }
}
