using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using paymatesapi.DTOs;
using paymatesapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserDTO request)
        {
            var response = await _userService.registerUser(request);
            if (response == null) return BadRequest(new { message = "Username or Email already exists" });
            return Ok(response);
        }

        [HttpPost("login")]
        public ActionResult Login(UserCreds creds)
        {
            //TODO: test this
            var response = _userService.loginUser(creds);
            if (response == null) return BadRequest(new { message = "Username or Password is incorrect" });
            return Ok(response);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
