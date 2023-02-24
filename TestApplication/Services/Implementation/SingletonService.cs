using Errorist;
using Errorist.Models;

namespace TestApplication.Services.Implementation
{
    public class SingletonService : ISingletonService
    {
        private readonly IExceptionScopeProviderFactory<ApiExceptionDto> _exceptionScopeProviderFactory;

        public SingletonService(IExceptionScopeProviderFactory<ApiExceptionDto> exceptionScopeProviderFactory)
        {
            _exceptionScopeProviderFactory = exceptionScopeProviderFactory;
        }

        public void PerformFunction(bool shouldFail)
        {
            using var scope = _exceptionScopeProviderFactory.CurrentProvider.GetScope();

            scope.ConfigureDefault()
                .AddConfiguration((e, dto) => dto.Message = "Thrown from a singleton service")
                .AddConfiguration((e, dto) => dto.StatusCode = 503);

            if (shouldFail)
            {
                throw new Exception("Some exception or other");
            }
        }
    }
}
