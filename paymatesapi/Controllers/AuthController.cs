using Microsoft.AspNetCore.Mvc;
using paymatesapi.Models;
using paymatesapi.DTOs;
using paymatesapi.Services;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Entities;
using paymatesapi.Helpers;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserAuthService userAuthService, IJwtUtils jwtUtils) : ControllerBase
    {
        private readonly IUserAuthService _userAuthService = userAuthService;
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse<User>>> Register(UserDTO request)
        {
            var response = await _userAuthService.RegisterUser(request); //returns null if user exists
            if (response.Error != null) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<User>>> Login(UserCreds creds)
        {
            var response = await _userAuthService.LoginUser(creds);
            if (response.Error != null) return BadRequest(
                new BaseResponse<User> { 
                    Error = new Error { Message = response.Error.Message } 
                }
            );
            return Ok(response);
        }

        [HttpPost("access-token")]
        public ActionResult<string> RefreshToken(RefreshTokenRequest requestBody)
        {
            BaseResponse<User> user = _userAuthService.GetUser(requestBody.Uid);
            if (user.Error != null) return BadRequest(user);
            if(user.Data == null) return BadRequest(user);
            if (!user.Data.RefreshToken.Equals(requestBody.RefreshToken))
            {
                return Unauthorized(new BaseResponse<User> { 
                    Error = new Error { Message = "Authenication Token is invalid" }
                });
            }
            else if (user.Data.RefreshTokenExpiry <  DateTime.Now.ToFileTimeUtc())
            {
                return Unauthorized(new BaseResponse<User> { 
                    Error = new Error { Message = "Session has expired" }
                });
            }

            var token = _jwtUtils.GenerateJwtToken(user.Data);
            var res = new BaseResponse<string> { Data = token };
            return Ok(res);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
