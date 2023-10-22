using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using paymatesapi.Services;


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

        [HttpPost("add-friend")]
        public ActionResult<string> AddFriend(FriendDTO request)
        {
            string user = _friendService.addFriend("friend");
            return Ok(user);
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
