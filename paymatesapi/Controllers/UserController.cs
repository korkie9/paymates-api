using Microsoft.AspNetCore.Mvc;
using paymatesapi.Models;
using paymatesapi.Services;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Entities;

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
            var response = _userAuthService.GetUser(userRequest.Uid);
            if (response.Error != null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
