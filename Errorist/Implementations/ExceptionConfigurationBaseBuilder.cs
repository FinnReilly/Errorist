namespace Errorist.Implementations
{
    public abstract class ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder> : IExceptionConfigurationBuilder<TOutput, TException>
        where TOutput : class
        where TException : class
        where TBuilder : class, IExceptionConfigurationBuilder<TOutput, TException>
    {
        public ExceptionConfigurationBaseBuilder()
        {
            Actions = new List<Action<Exception, TOutput>>();
            ExceptionType = typeof(TException);
        }

        public IEnumerable<Action<Exception, TOutput>> Actions { get; }

        public Type ExceptionType { get; }
    }
}
