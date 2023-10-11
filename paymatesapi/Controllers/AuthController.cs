using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static UserDto user = new UserDto
        {
            Email = "justin@korkie.cm",
            Password = "123"
        };
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAuth()
        {

            return Ok(user);
        }
    }
}
