using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using paymatesapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserModel request)
        {
            var response = await _userService.registerUser(request);
            if (response == null) return BadRequest(new { message = "Username or Email already exists" });
            return Ok(response);
        }

        [HttpPost("login")]
        // public Task<ActionResult<AuthenticationResponse>> Login(UserCreds creds)
        public ActionResult Login(UserCreds creds)

        {
            var response = _userService.loginUser(creds);
            if (response == null) return BadRequest(new { message = "Username or Password is incorrect" });
            // return Ok(response);
            return Ok();
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
