using Errorist.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Errorist.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IExceptionFormattingService<>), typeof(ExceptionFormattingService<>));
            services.AddSingleton(typeof(IHttpContextConfigurator<>), typeof(DefaultContextConfigurator<>));

            return services;
        }
    }
}
