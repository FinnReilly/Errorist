using Errorist.Implementations;
using Errorist.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Errorist.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorConfigurationMiddleware<TOutput>(this IApplicationBuilder builder)
            where TOutput : class, new()
            => builder.UseMiddleware<ExceptionHandlingMiddleware<TOutput>>();

        public static GenericExceptionConfigurationBuilder<TOutput, TException> UseGlobalErrorConfiguration<TOutput, TException>(this IApplicationBuilder builder)
            where TOutput : class
            where TException : Exception
            => builder.ApplicationServices
                .GetRequiredService<IExceptionFormattingGlobalScope<TOutput>>()
                .Configure<TException>();

        public static GenericExceptionConfigurationBuilder<TOutput, Exception> UseGlobalDefaultErrorConfiguration<TOutput>(this IApplicationBuilder builder)
            where TOutput : class
            => builder.ApplicationServices
                .GetRequiredService<IExceptionFormattingGlobalScope<TOutput>>()
                .ConfigureDefault();

        public static TBuilder UseGlobalErrorConfigurationWithBuilder<TBuilder, TOutput, TException>(this IApplicationBuilder builder)
            where TOutput : class
            where TException : Exception
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>
            => builder.ApplicationServices
                .GetRequiredService<IExceptionFormattingGlobalScope<TOutput>>()
                .ConfigureWithBuilder<TBuilder, TException>();

        public static TBuilder UseGlobalDefaultErrorConfigurationWithBuilder<TBuilder, TOutput>(this IApplicationBuilder builder)
            where TOutput : class
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, Exception, TBuilder>
            => builder.ApplicationServices
                .GetRequiredService<IExceptionFormattingGlobalScope<TOutput>>()
                .ConfigureDefaultWithBuilder<TBuilder>();
    }
}
