using Microsoft.AspNetCore.Mvc;
using paymatesapi.Models;
using paymatesapi.Services;

namespace paymatesapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IConfiguration configuration, IEmailService emailService) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IEmailService _emailService = emailService;

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<string> Get()
    {

        return Ok(_configuration.GetSection("Weather:Test").Value!);
    }

    [HttpPost("email")]
    public IActionResult SendMail(EmailBody email)
    {
        return Ok(_emailService.SendEmail(email));
    }
}
