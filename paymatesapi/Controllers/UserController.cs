using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using paymatesapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Helpers;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public UserController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("get-user"), Authorize]
        public ActionResult<UserResponse> GetUser(UserRequest userRequest)
        {
            var response = _userAuthService.getUser(userRequest.Uid);
            if (response == null || response.Uid == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(response);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
