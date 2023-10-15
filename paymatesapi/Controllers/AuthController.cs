using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using paymatesapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public async Task<ActionResult<AuthenticationResponse>> Register(User request)
        {
            var response = await _userService.registerUser(request);
            if (response == null) return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response);
        }
    }
}
