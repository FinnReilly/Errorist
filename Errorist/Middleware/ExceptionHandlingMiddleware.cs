using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Errorist.Middleware
{
    public class ExceptionHandlingMiddleware<TOutput>
        where TOutput : class, new()
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionFormattingService<TOutput> _formattingService;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IExceptionFormattingService<TOutput> formattingService)
        {
            _next = next;
            _formattingService = formattingService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var configuredOutput = _formattingService.Configure(new TOutput(), e);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(configuredOutput));
            }
        }
    }
}
