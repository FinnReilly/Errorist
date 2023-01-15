using Errorist.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Errorist.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorConfiguration(this IApplicationBuilder builder)
        {
            var configuredErrorConfigService = builder.ApplicationServices.GetRequiredService(typeof(IExceptionFormattingService<>));
            if (configuredErrorConfigService != null)
            {
                var type = configuredErrorConfigService.GetType().GetGenericArguments().First();
                var middlewareType = typeof(ExceptionHandlingMiddleware<>).MakeGenericType(type);
                return builder.UseMiddleware(middlewareType);
            }

            throw new InvalidOperationException(
                "No error configuration services have been registered -" +
                " please call services.AddErrorConfiguration<TOutput>() in your service configuration code.");
        }
    }
}
