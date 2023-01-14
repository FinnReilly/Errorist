using Errorist.Models;

namespace Errorist.Implementations
{
    public class ApiExceptionDtoConfigurationBuilder<TException> : ExceptionConfigurationBaseBuilder<ApiExceptionDto, TException, ApiExceptionDtoConfigurationBuilder<TException>>
        where TException : Exception
    {
        public ApiExceptionDtoConfigurationBuilder() 
            : base()
        {
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithTitle(string title)
        {
            AddConfiguration((e, dto) => dto.Title = title);
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithTitle(Func<TException, string> titleFactory)
        {
            AddConfiguration((e, dto) => dto.Title = titleFactory(e));
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithMessage(string message)
        {
            AddConfiguration((e, dto) => dto.Message = message);
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithMessage(Func<TException, string> messageFactory)
        {
            AddConfiguration((e, dto) => dto.Message = messageFactory(e));
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithUserAdvice(string userAdvice)
        {
            AddConfiguration((e, dto) => dto.UserAdvice = userAdvice);
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithUserAdvice(Func<TException, string> userAdviceFactory)
        {
            AddConfiguration((e, dto) => dto.UserAdvice = userAdviceFactory(e));
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithStatusCode(int statusCode)
        {
            AddConfiguration((e, dto) => dto.StatusCode = statusCode);
            return this;
        }

        ApiExceptionDtoConfigurationBuilder<TException> WithStatusCode(Func<TException, int> statusCodeFactory)
        {
            AddConfiguration((e, dto) => dto.StatusCode = statusCodeFactory(e));
            return this;
        }
    }
}
