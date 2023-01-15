using Errorist.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Errorist.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorConfiguration<TOutput>(this IApplicationBuilder builder)
            where TOutput : class, new()
            => builder.UseMiddleware<ExceptionHandlingMiddleware<TOutput>>();
    }
}
