namespace Errorist
{
    public interface IExceptionConfigurationBuilder<TOutput>
        where TOutput : class
    {
        IEnumerable<Action<Exception, TOutput>> Actions { get; }
        Type ExceptionType { get; }
    }

    public interface IExceptionConfigurationBuilder<TOutput, TException> : IExceptionConfigurationBuilder<TOutput>
        where TOutput : class
    { }
}
