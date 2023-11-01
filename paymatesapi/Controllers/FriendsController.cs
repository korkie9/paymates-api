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
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService _friendService;
        private readonly IJwtUtils _jwtUtils;

        public FriendsController(IFriendService friendService, IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
            _friendService = friendService;
        }

        [HttpPost("add-friend"), Authorize]
        public async Task<ActionResult<string>> AddFriend(FriendDTO request)
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (String.IsNullOrEmpty(userId)) return Unauthorized(new { message = "User is not authenticated" });

            string user = await _friendService.addFriend(userId, request.FriendUid);
            if (user == "Users are already friends") return BadRequest(new { message = user });
            if (user == "User not found") return BadRequest(new { message = user });
            return Ok(user); //TODO: return okay responses with object containing message for uniformity across project
        }

        [HttpDelete("remove-friend"), Authorize]
        public async Task<IActionResult> RemoveFriend(FriendDTO creds)
        {
            //TODO: Once transactions are created, this controller must be updated to delete ascociated transactions
            var userId = _jwtUtils.GetUidFromHeaders();
            if (String.IsNullOrEmpty(userId)) return Unauthorized(new { message = "User is not authenticated" });

            var userIsDeleted = await _friendService.deleteFriend(creds.FriendUid);
            if (userIsDeleted) return Ok("Friend was removed");
            return BadRequest("An error occured. Friend could not be deleted.");
        }

        [HttpGet("get-friends"), Authorize]
        public ActionResult<List<UserResponse>> GetUserFriends()
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (String.IsNullOrEmpty(userId)) return Unauthorized(new { message = "User is not authenticated" });
            var friends = _friendService.GetFriendsOfUser(userId);
            if (friends == null) return BadRequest("An error occured. Friend could not be found.");
            return Ok(friends);
        }
        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }


    }
}
