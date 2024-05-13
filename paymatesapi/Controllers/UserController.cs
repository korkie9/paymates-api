using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paymatesapi.Entities;
using paymatesapi.Models;
using paymatesapi.Services;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserAuthService userAuthService) : ControllerBase
    {
        private readonly IUserAuthService _userAuthService = userAuthService;

        [HttpPost("get-user"), Authorize]
        public ActionResult<BaseResponse<User>> GetUser(UserRequest userRequest)
        {
            BaseResponse<User> response = _userAuthService.GetUser(userRequest.Uid);
            return response.Error != null ? BadRequest(response) : Ok(response);
        }

        [HttpPost("update-user"), Authorize]
        public async Task<ActionResult<BaseResponse<bool>>> UpdateUser(
            UserUpdateRequest userRequest
        )
        {
            BaseResponse<bool> response = await _userAuthService.UpdateUser(userRequest);
            return response.Error != null ? BadRequest(response) : Ok(response);
        }
    }
}
