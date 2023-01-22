using Errorist.Models;

namespace Errorist.Implementations
{
    public class ApiExceptionDtoConfigurationBuilder<TException> : ExceptionConfigurationBaseBuilder<ApiExceptionDto, TException, ApiExceptionDtoConfigurationBuilder<TException>>
        where TException : Exception
    {
        public ApiExceptionDtoConfigurationBuilder<TException> WithTitle(string title)
            => AddConfiguration((e, dto) => dto.Title = title);             

        public ApiExceptionDtoConfigurationBuilder<TException> WithTitle(Func<TException, string> titleFactory)
            => AddConfiguration((e, dto) => dto.Title = titleFactory(e));

        public ApiExceptionDtoConfigurationBuilder<TException> WithMessage(string message)
            => AddConfiguration((e, dto) => dto.Message = message);

        public ApiExceptionDtoConfigurationBuilder<TException> WithMessage(Func<TException, string> messageFactory)
            => AddConfiguration((e, dto) => dto.Message = messageFactory(e));

        public ApiExceptionDtoConfigurationBuilder<TException> WithUserAdvice(string userAdvice)
            => AddConfiguration((e, dto) => dto.UserAdvice = userAdvice);

        public ApiExceptionDtoConfigurationBuilder<TException> WithUserAdvice(Func<TException, string> userAdviceFactory)
            => AddConfiguration((e, dto) => dto.UserAdvice = userAdviceFactory(e));

        public ApiExceptionDtoConfigurationBuilder<TException> WithStatusCode(int statusCode)
            => AddConfiguration((e, dto) => dto.StatusCode = statusCode);

        public ApiExceptionDtoConfigurationBuilder<TException> WithStatusCode(Func<TException, int> statusCodeFactory)
            => AddConfiguration((e, dto) => dto.StatusCode = statusCodeFactory(e));
    }
}
