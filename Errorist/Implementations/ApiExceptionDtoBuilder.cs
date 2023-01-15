using Errorist.Models;

namespace Errorist.Implementations
{
    public class ApiExceptionDtoConfigurationBuilder<TException> : ExceptionConfigurationBaseBuilder<ApiExceptionDto, TException, ApiExceptionDtoConfigurationBuilder<TException>>
        where TException : Exception
    {
        ApiExceptionDtoConfigurationBuilder<TException> WithTitle(string title)
            => AddConfiguration((e, dto) => dto.Title = title);             

        ApiExceptionDtoConfigurationBuilder<TException> WithTitle(Func<TException, string> titleFactory)
            => AddConfiguration((e, dto) => dto.Title = titleFactory(e));

        ApiExceptionDtoConfigurationBuilder<TException> WithMessage(string message)
            => AddConfiguration((e, dto) => dto.Message = message);

        ApiExceptionDtoConfigurationBuilder<TException> WithMessage(Func<TException, string> messageFactory)
            => AddConfiguration((e, dto) => dto.Message = messageFactory(e));

        ApiExceptionDtoConfigurationBuilder<TException> WithUserAdvice(string userAdvice)
            => AddConfiguration((e, dto) => dto.UserAdvice = userAdvice);

        ApiExceptionDtoConfigurationBuilder<TException> WithUserAdvice(Func<TException, string> userAdviceFactory)
            => AddConfiguration((e, dto) => dto.UserAdvice = userAdviceFactory(e));

        ApiExceptionDtoConfigurationBuilder<TException> WithStatusCode(int statusCode)
            => AddConfiguration((e, dto) => dto.StatusCode = statusCode);

        ApiExceptionDtoConfigurationBuilder<TException> WithStatusCode(Func<TException, int> statusCodeFactory)
            => AddConfiguration((e, dto) => dto.StatusCode = statusCodeFactory(e));
    }
}
