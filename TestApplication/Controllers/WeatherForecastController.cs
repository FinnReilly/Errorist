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
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm"
        };

        private static readonly int[] Temperatures = new[]
        {
            -10, -3, 1, 5, 10, 18
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IExceptionScopeProvider<ApiExceptionDto> _exceptions;
        private readonly IService _service;
        private readonly IServiceWithTryCatch _serviceWithTryCatch;
        private readonly ISingletonService _singletonService;

        private readonly DateTime _standardTime = new DateTime(1918, 11, 11, 11, 00, 00);

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IExceptionScopeProvider<ApiExceptionDto> formattingService,
            IService service,
            IServiceWithTryCatch serviceWithTryCatch,
            ISingletonService singletonService)
        {
            _logger = logger;
            _exceptions = formattingService;
            _service = service;
            _serviceWithTryCatch = serviceWithTryCatch;
            _singletonService = singletonService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(bool shouldFailInService, bool shouldFailInSingletonService, bool shouldFailInController)
        {
            using var exceptionScope = _exceptions.GetScope();
            exceptionScope.ConfigureDefault()
                .AddConfiguration((exception, dto) =>
                {
                    dto.Title = "Error on forecast controller";
                    dto.Message = $"{dto.Message}:{exception.Message}";
                });


            _service.PerformFunction(shouldFailInService);

            _singletonService.PerformFunction(shouldFailInSingletonService);

            if (shouldFailInController)
            {
                throw new ArgumentException("WRONG!");
            }

            return Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = _standardTime,
                    TemperatureC = Temperatures[index],
                    Summary = Summaries[index]
                })
                .ToArray();
        }

        [Route("withTryCatch")]
        [HttpGet]
        public IEnumerable<WeatherForecast> GetWithTryCatch(bool shouldFailTopLevel, bool shouldFailInServiceAndRethrow)
        {
            using var scope = _exceptions.GetScope();
            scope.ConfigureDefault()
                .AddConfiguration((e, dto) => 
                {
                    dto.Title = $"Error in GetWithTryCatch Endpoint";
                    dto.Message = $"Error in GetWithTryCatch Endpoint --> {dto.Message}";
                });

            try
            {
                _serviceWithTryCatch.PerformAction(shouldFailTopLevel || shouldFailInServiceAndRethrow);
            }
            catch
            {
                if (shouldFailInServiceAndRethrow)
                {
                    throw;
                }
            }

            if (shouldFailTopLevel)
            {
                throw new Exception("Message is not important here");
            }

            return Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = _standardTime,
                    TemperatureC = Temperatures[index],
                    Summary = Summaries[index]
                })
                .ToArray();
        }
    }
}