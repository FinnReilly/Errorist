using Errorist.Implementations;

namespace Errorist
{
    public interface IConfigurationBuilderFactory
    {
        TBuilder Create<TBuilder, TOutput, TException>()
            where TOutput : class
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>
            where TException : Exception;
    }
}
