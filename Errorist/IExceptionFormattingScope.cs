using Errorist.Implementations;

namespace Errorist
{
    public interface IExceptionFormattingScope<TOutput> : IDisposable
        where TOutput : class
    {
        GenericExceptionConfigurationBuilder<TOutput, Exception> ConfigureDefault();
        GenericExceptionConfigurationBuilder<TOutput, TException> Configure<TException>()
            where TException : Exception;
        TBuilder ConfigureDefaultWithBuilder<TBuilder>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, Exception, TBuilder>, new();
        TBuilder ConfigureWithBuilder<TBuilder, TException>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>, new()
            where TException : Exception;

        void Complete();
    }
}
