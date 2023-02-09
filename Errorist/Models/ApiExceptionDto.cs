using Newtonsoft.Json;

namespace Errorist.Models
{
    public class ApiExceptionDto : IHasStatusCode
    {
        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? UserAdvice { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }
    }
}
