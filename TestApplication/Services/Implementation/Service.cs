using Errorist;
using Errorist.Implementations;
using Errorist.Models;

namespace TestApplication.Services.Implementation
{
    public class Service : IService
    {
        private readonly IExceptionScopeProvider<ApiExceptionDto> _exceptions;

        public Service(IExceptionScopeProvider<ApiExceptionDto> exceptions)
        {
            _exceptions = exceptions;
        }

        public void PerformFunction(bool shouldFail)
        {
            using var errorScope = _exceptions.GetScope();
            errorScope.ConfigureDefaultWithBuilder<ApiExceptionDtoConfigurationBuilder<Exception>>()
                .WithMessage("Something went wrong in an IService");

            if (shouldFail)
            {
                throw new NotImplementedException();
            }
        }
    }
}
