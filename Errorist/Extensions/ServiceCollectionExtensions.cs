using Errorist.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Errorist.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorConfiguration<TOutput>(this IServiceCollection services)
            where TOutput : class, new()
        {
            services.AddScoped<IExceptionScopeProvider<TOutput>, ExceptionScopeProvider<TOutput>>();
            services.AddScoped<IExceptionOutputConfigurator<TOutput>, ExceptionOutputConfigurator<TOutput>>();
            services.AddScoped<IScopedConfigurationQueue<TOutput>, ScopedConfigurationQueue<TOutput>>();
            services.AddSingleton<IHttpContextConfigurator<TOutput>, DefaultContextConfigurator<TOutput>>();
            services.AddSingleton<IExceptionFormattingGlobalScope<TOutput>, ExceptionFormattingGlobalScope<TOutput>>();
            services.AddSingleton<IConfigurationBuilderFactory, ConfigurationBuilderFactory>();

            return services;
        }
    }
}
