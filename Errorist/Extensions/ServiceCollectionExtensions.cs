using Errorist.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Errorist.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorConfiguration<TOutput>(this IServiceCollection services)
            where TOutput : class, new()
        {
            services.AddScoped<IExceptionFormattingService<TOutput>, ExceptionFormattingService<TOutput>>();
            services.AddSingleton<IHttpContextConfigurator<TOutput>, DefaultContextConfigurator<TOutput>>();
            services.AddSingleton<IExceptionFormattingGlobalScope<TOutput>, ExceptionFormattingGlobalScope<TOutput>>();
            services.AddSingleton<IConfigurationBuilderFactory, ConfigurationBuilderFactory>();

            return services;
        }
    }
}
