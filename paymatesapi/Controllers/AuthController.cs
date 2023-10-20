using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using paymatesapi.DTOs;
using paymatesapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Entities;
using paymatesapi.Helpers;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtUtils _jwtUtils;

        public AuthController(IUserService userService, IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserDTO request)
        {
            var response = await _userService.registerUser(request); //returns null if user exists
            //TODO: clean this up somehow?
            if(response.Email == null) return BadRequest(new { message = "Username or Email already exists" });
            if(response.Username == null) return BadRequest(new { message = "Username or Email already exists" });
            if(response.Uid == null) return BadRequest(new { message = "Username or Email already exists" });
            if (response == null) return BadRequest(new { message = "Username or Email already exists" });
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCreds creds)
        {
            //TODO: test this
            var response = await _userService.loginUser(creds);
            if (response == null) return BadRequest(new { message = "Username or Password is incorrect" });
            return Ok(response);
        }

        [HttpGet("refresh-token")]
        public ActionResult<string> RefreshToken(User user)
        {
            AuthenticationResponse response = _userService.getUser(user.Uid, user.Email, user.Username);
            if(response.Uid == null) return BadRequest("User does not exist");
            if (response.RefreshToken == null) return BadRequest("Invalid Refresh token");
            if (!response.RefreshToken.Equals(user.RefreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(response?.RefreshTokenExpiry < DateTime.Now)
            {
                return Unauthorized("Session has expired.");
            }

            string token = _jwtUtils.GenerateJwtToken(user);

            return Ok(token);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
