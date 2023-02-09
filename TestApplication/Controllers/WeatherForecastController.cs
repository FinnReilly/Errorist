using Errorist;
using Errorist.Models;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Services;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IExceptionFormattingService<ApiExceptionDto> _exceptions;
        private readonly IService _service;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IExceptionFormattingService<ApiExceptionDto> formattingService,
            IService service)
        {
            _logger = logger;
            _exceptions = formattingService;
            _service = service;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(bool shouldFailInService, bool shouldFailInController)
        {
            using var exceptionScope = _exceptions.GetScope();
            exceptionScope.ConfigureDefault()
                .AddConfiguration((exception, dto) =>
                {
                    dto.Title = "Error on forecast controller";
                    dto.Message = $"{dto.Message}:{exception.Message}";
                });

            _service.PerformFunction(shouldFailInService);

            var weatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            if (shouldFailInController)
            {
                throw new ArgumentException("WRONG!");
            }
            exceptionScope.Complete();

            return weatherForecast;
        }
    }
}