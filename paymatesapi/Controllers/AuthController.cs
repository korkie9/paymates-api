using Microsoft.AspNetCore.Mvc;
using paymatesapi.Models;
using paymatesapi.DTOs;
using paymatesapi.Services;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Entities;
using paymatesapi.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserAuthService userAuthService, IJwtUtils jwtUtils) : ControllerBase
    {
        private readonly IUserAuthService _userAuthService = userAuthService;
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        [HttpPost("register")]
        public ActionResult<BaseResponse<User>> Register(UserDTO request)
        {
            var response = _userAuthService.RegisterUser(request);
            // if (response.Error != null) return Ok(response);
            return Ok(response);
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<BaseResponse<User>>> CreateUser(CreateUserDto userdto)
        {
            var response = await _userAuthService.CreateUser(userdto.Token);
            if (response.Error != null) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse<User>>> Login(UserCreds creds)
        {
            var response = await _userAuthService.LoginUser(creds);
            if (response.Error != null) return BadRequest(
                new BaseResponse<User>
                {
                    Error = new Error { Message = response.Error.Message }
                }
            );
            return Ok(response);
        }

        [HttpPost("access-token")]
        public ActionResult<string> AccessToken(RefreshTokenRequest requestBody)
        {
            BaseResponse<User> user = _userAuthService.GetUser(requestBody.Uid);
            if (user.Error != null) return BadRequest(user);
            if (user.Data == null) return BadRequest(user);
            if (!user.Data.RefreshToken.Equals(requestBody.RefreshToken))
            {
                return Unauthorized(new BaseResponse<User>
                {
                    Error = new Error { Message = "User is not authenticated" }
                });
            }

            else if (user.Data.RefreshTokenExpiry < DateTime.Now.ToFileTimeUtc())
            {
                return Unauthorized(new BaseResponse<User>
                {
                    Error = new Error { Message = "Session has expired" }
                });
            }

            var token = _jwtUtils.GenerateJwtToken(user.Data, 5);
            var res = new BaseResponse<string> { Data = token };
            return Ok(res);
        }

        [HttpPost("rotate-refresh-token")]
        public async Task<ActionResult<BaseResponse<User>>> RotateToken(RefreshTokenRequest requestBody)
        {
            var res = await _userAuthService.UpdateRefreshToken(requestBody);
            if (res.Error != null) return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("test"), Authorize]
        public ActionResult<string> Test()
        {
            return Ok("hello world");
        }
    }
}
