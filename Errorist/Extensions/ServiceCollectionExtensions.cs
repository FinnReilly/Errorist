using Errorist.Implementations;
using Microsoft.AspNetCore.Http;
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
            services.AddSingleton<IExceptionScopeProviderFactory<TOutput>, ExceptionScopeProviderFactory<TOutput>>();
            services.AddSingleton<IHttpContextConfigurator<TOutput>, DefaultContextConfigurator<TOutput>>();
            services.AddSingleton<IExceptionFormattingGlobalScope<TOutput>, ExceptionFormattingGlobalScope<TOutput>>();
            services.AddSingleton<IConfigurationBuilderFactory, ConfigurationBuilderFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
