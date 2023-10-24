using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using paymatesapi.Services;
using System.IdentityModel.Tokens.Jwt;



namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendsController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("add-friend"), Authorize]
        public async Task<ActionResult<string>> AddFriend(FriendDTO request)
        {
            if (Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            {
                string authHeaderValue = authHeaderValues.FirstOrDefault();

                if (!string.IsNullOrEmpty(authHeaderValue) && authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = authHeaderValue.Substring("Bearer ".Length);

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    if (securityToken == null) return BadRequest(new { message = "User is not authorized" });
                    var claims = securityToken.Claims;
                    var userId = claims.FirstOrDefault(c => c.Type == "Uid")?.Value;
                    string user = await _friendService.addFriend(userId, request.FriendUid);
                    if (user == null) return BadRequest(new { message = "Users are already friends" });
                    if (user == "User not found") return BadRequest(new { message = user });
                    return Ok(user);
                }
            }
            return StatusCode(500);
        }

        [HttpPost("remove-friend"), Authorize]
        public ActionResult<string> RemoveFriend(FriendDTO creds)
        {
            string user = _friendService.deleteFriend("friend");
            return Ok(user);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
