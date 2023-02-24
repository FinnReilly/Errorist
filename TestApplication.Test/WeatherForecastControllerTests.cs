using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TestApplication.Test
{
    [TestFixture]
    public class WeatherForecastControllerTests
    {
        private HttpClient _httpClient;
        private Random _random;

        [SetUp]
        public void Setup()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
            _random = new Random();
        }

        private static IEnumerable<(string url, string jsonOutput, HttpStatusCode statusCode)> AllScenariosAndOutcomes = new List<(string, string, HttpStatusCode)>
        {
            (
                "/weatherforecast",
                "[{\"date\":\"1918-11-11T11:00:00\",\"temperatureC\":-3,\"temperatureF\":27,\"summary\":\"Bracing\"}," +
                    "{\"date\":\"1918-11-11T11:00:00\",\"temperatureC\":1,\"temperatureF\":33,\"summary\":\"Chilly\"}," +
                    "{\"date\":\"1918-11-11T11:00:00\",\"temperatureC\":5,\"temperatureF\":40,\"summary\":\"Cool\"}," +
                    "{\"date\":\"1918-11-11T11:00:00\",\"temperatureC\":10,\"temperatureF\":49,\"summary\":\"Mild\"}," +
                    "{\"date\":\"1918-11-11T11:00:00\",\"temperatureC\":18,\"temperatureF\":64,\"summary\":\"Warm\"}]",
                HttpStatusCode.OK),
            (
                "/weatherforecast?shouldFailInController=true",
                "{\"Title\":\"Error on forecast controller\",\"Message\":\"Something went wrong:WRONG!\",\"UserAdvice\":\"Hang tight and we'll be right with you\"}",
                HttpStatusCode.InternalServerError),
            (
                "/weatherforecast?shouldFailInSingletonService=true",
                "{\"Title\":\"Error on forecast controller\",\"Message\":\"Thrown from a singleton service:Some exception or other\",\"UserAdvice\":\"Hang tight and we'll be right with you\"}",
                HttpStatusCode.ServiceUnavailable),
            (
                "/weatherforecast?shouldFailInService=true",
                "{\"Title\":\"Error on forecast controller\",\"Message\":\"Something went wrong in an IService:The method or operation is not implemented.\",\"UserAdvice\":\"Hang tight and we'll be right with you\"}",
                HttpStatusCode.InternalServerError)
        };

        private static IEnumerable<TestCaseData> AllSingleThreadedTestCases()
        {
            var testCaseData = AllScenariosAndOutcomes
                .Select(data => new TestCaseData(data.url, data.jsonOutput, data.statusCode));
            foreach(var testCase in testCaseData)
            {
                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(AllSingleThreadedTestCases))]
        public async Task GetWeatherForecast_AllScenarios_ReturnExpectedOutput(
            string urlToCall,
            string expectedJsonOutput,
            HttpStatusCode expectedHttpStatusCode)
        {
            // Act
            var result = await _httpClient.GetAsync(urlToCall);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(expectedHttpStatusCode));
            var contentAsString = await result.Content.ReadAsStringAsync();
            Assert.That(contentAsString, Is.EqualTo(expectedJsonOutput));
        }

        [Test]
        public void GetWeatherForecast_InMultithreadedScenario_ReturnsCorrectDataForEachRequest()
        {
            // Arrange
            Parallel.For(0, 100, i => 
            {
                Parallel.ForEach(AllScenariosAndOutcomes, async (scenario, token) => 
                {
                    await Task.Delay(_random.Next(0, 5));

                    // Act
                    var result = await _httpClient.GetAsync(scenario.url);
                    var contentAsString = await result.Content.ReadAsStringAsync();

                    // Assert
                    Assert.That(result.StatusCode, Is.EqualTo(scenario.statusCode));
                    Assert.That(contentAsString, Is.EqualTo(scenario.jsonOutput));
                });
            });
        }
    }
}
