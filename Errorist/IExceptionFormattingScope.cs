using Errorist.Implementations;

namespace Errorist
{
    public interface IExceptionFormattingScope<TOutput>
        where TOutput : class
    {
        GenericExceptionConfigurationBuilder<TOutput, Exception> ConfigureDefault();
        GenericExceptionConfigurationBuilder<TOutput, TException> Configure<TException>()
            where TException : Exception;
        TBuilder ConfigureDefaultWithBuilder<TBuilder>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, Exception, TBuilder>;
        TBuilder ConfigureWithBuilder<TBuilder, TException>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>
            where TException : Exception;
    }
}
