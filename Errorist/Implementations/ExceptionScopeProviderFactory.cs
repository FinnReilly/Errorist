using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Errorist.Implementations
{
    public class ExceptionScopeProviderFactory<TOutput> : IExceptionScopeProviderFactory<TOutput>
        where TOutput : class, new()
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ExceptionScopeProviderFactory(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public IExceptionScopeProvider<TOutput> CurrentProvider => _contextAccessor
            .HttpContext
            .RequestServices
            .GetRequiredService<IExceptionScopeProvider<TOutput>>();
    }
}
