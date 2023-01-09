namespace Errorist.Models
{
    public class ApiExceptionDto
    {
        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? UserAdvice { get; set; }

        public int StatusCode { get; set; }
    }
}
