using Microsoft.AspNetCore.Mvc;

namespace paymatesapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController() : ControllerBase
{

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<WeatherForecast> Get()
    {
        return Ok();
    }
}
