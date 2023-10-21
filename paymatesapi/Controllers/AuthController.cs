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
            if (response == null) return BadRequest(new { message = "Username or Email already exists" });
            if (response.Email == null) return BadRequest(new { message = "Username or Email already exists" });
            if (response.Username == null) return BadRequest(new { message = "Username or Email already exists" });
            if (response.Uid == null) return BadRequest(new { message = "Username or Email already exists" });
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCreds creds)
        {
            var response = await _userService.loginUser(creds);
            if (response.Uid == null) return BadRequest(new { message = "Username or Password is incorrect" });
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public ActionResult<string> RefreshToken(RefreshTokenRequest requestBody)
        {
            AuthenticationResponse user = _userService.getUser(requestBody.Uid);
            if (user == null) return BadRequest("User does not exist");
            if (user.Uid == null) return BadRequest("User does not exist");
            if (user.RefreshToken == null) return BadRequest(new { message = "Invalid Refresh token" });
            if (!user.RefreshToken.Equals(requestBody.RefreshToken))
            {
                return Unauthorized(new { message = "Invalid Refresh Token." });
            }
            else if (user?.RefreshTokenExpiry < DateTime.Now)
            {
                return Unauthorized(new { message = "Session has expired." });
            }

            string token = _jwtUtils.GenerateJwtToken(user);
            if (token == "error") return BadRequest(new { message = "There was an issue generating your refresh token" });
            return Ok(token);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
