using Microsoft.AspNetCore.Mvc;
using textforum.data.contexts;
using textforum.domain.interfaces;
using textforum.logic.filters;
using textforum.logic.services;

namespace textforum.api.Controllers
{

    [AppAuthAttribute]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAppAuthenticationService _appAuthenticationService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get([FromHeader(Name = "X-Token")] string token, [FromHeader(Name = "X-Forwarded-For")] string ip, [FromHeader(Name = "X-Machine-Name")] string machineName)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
