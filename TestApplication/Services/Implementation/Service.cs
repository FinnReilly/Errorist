using Errorist;
using Errorist.Implementations;
using Errorist.Models;

namespace TestApplication.Services.Implementation
{
    public class Service : IService
    {
        private readonly IExceptionFormattingService<ApiExceptionDto> _exceptions;

        public Service(IExceptionFormattingService<ApiExceptionDto> exceptions)
        {
            _exceptions = exceptions;
        }

        public void PerformFunction()
        {
            using var errorScope = _exceptions.GetScope();
            errorScope.ConfigureDefaultWithBuilder<ApiExceptionDtoConfigurationBuilder<Exception>>()
                .WithMessage("Something went wrong in an IService");

            throw new NotImplementedException();
        }
    }
}
