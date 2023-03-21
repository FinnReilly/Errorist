using Errorist;
using Errorist.Models;

namespace TestApplication.Services.Implementation
{
    public class ServiceWithTryCatch : IServiceWithTryCatch
    {
        private readonly IService _service;
        private readonly IExceptionScopeProvider<ApiExceptionDto> _exceptions;

        public ServiceWithTryCatch(
            IService service,
            IExceptionScopeProvider<ApiExceptionDto> exceptionScopeProvider)
        {
            _service = service;
            _exceptions = exceptionScopeProvider;
        }

        public void PerformAction(bool shouldFailAfterTryCatch)
        {
            using var scope = _exceptions.GetScope();
            scope.ConfigureDefault()
                .AddConfiguration((e, dto) =>
                {
                    dto.Message = $"Try catch service --> {dto.Message}";
                });

            try
            {
                _service.PerformFunction(shouldFail: true);
            }
            catch
            {
            }

            if (shouldFailAfterTryCatch)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
