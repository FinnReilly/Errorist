namespace Errorist.Implementations
{
    public abstract class ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder> : IExceptionConfigurationBuilder<TOutput, TException>
        where TOutput : class
        where TException : Exception
        where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>
    {
        private List<Action<Exception, TOutput>> _configurationActions;

        public ExceptionConfigurationBaseBuilder()
        {
            _configurationActions = new List<Action<Exception, TOutput>>();
            ExceptionType = typeof(TException);
        }

        public IEnumerable<Action<Exception, TOutput>> Actions => _configurationActions;

        public Type ExceptionType { get; }

        public TBuilder AddConfiguration(Action<TException, TOutput> configurationAction)
        {
            _configurationActions.Add((exception, output) =>
            {
                if (configurationAction != null
                    && exception is TException exceptionAsType)
                {
                    configurationAction(exceptionAsType, output);
                }
            });

            if (this is TBuilder tBuilder)
            {
                return tBuilder;
            }

            throw new InvalidOperationException();
        }
    }
}
