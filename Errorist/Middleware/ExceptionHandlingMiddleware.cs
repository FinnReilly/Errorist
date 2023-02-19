using Microsoft.AspNetCore.Http;

namespace Errorist.Middleware
{
    public class ExceptionHandlingMiddleware<TOutput>
        where TOutput : class, new()
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IExceptionOutputConfigurator<TOutput> formattingService,
            IHttpContextConfigurator<TOutput> contextConfigurator)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var configuredOutput = formattingService.Configure(new TOutput(), e);
                await contextConfigurator.ConfigureContextWithErrorResponse(context, configuredOutput);
            }
        }
    }
}
