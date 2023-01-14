namespace Errorist
{
    public interface IExceptionConfigurationBuilder<TOutput>
        where TOutput : class
    {
        IEnumerable<Action<Exception, TOutput>> Actions { get; }
        Type ExceptionType { get; }
        IExceptionConfigurationBuilder<TOutput, TException> AsType<TException>()
            where TException : Exception;
    }

    public interface IExceptionConfigurationBuilder<TOutput, TException> : IExceptionConfigurationBuilder<TOutput>
        where TOutput : class
        where TException : Exception
    { }
}
