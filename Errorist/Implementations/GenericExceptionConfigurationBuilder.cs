namespace Errorist.Implementations
{
    public class GenericExceptionConfigurationBuilder<TOutput, TException> : ExceptionConfigurationBaseBuilder<TOutput, TException, GenericExceptionConfigurationBuilder<TOutput, TException>>
        where TException : Exception
        where TOutput : class
    {
    }
}
