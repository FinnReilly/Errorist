using Microsoft.AspNetCore.Http;

namespace Errorist.Middleware
{
    public class ExceptionHandlingMiddleware<TOutput>
        where TOutput : class, new()
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionFormattingService<TOutput> _formattingService;
        private readonly IHttpContextConfigurator<TOutput> _contextConfigurator;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IExceptionFormattingService<TOutput> formattingService,
            IHttpContextConfigurator<TOutput> contextConfigurator)
        {
            _next = next;
            _formattingService = formattingService;
            _contextConfigurator = contextConfigurator;
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
                await _contextConfigurator.ConfigureContextWithErrorResponse(context, configuredOutput);
            }
        }
    }
}
