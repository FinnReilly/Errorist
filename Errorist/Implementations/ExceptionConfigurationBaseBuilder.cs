namespace Errorist.Implementations
{
    public abstract class ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder> : IExceptionConfigurationBuilder<TOutput, TException>
        where TOutput : class
        where TException : Exception
        where TBuilder : class, IExceptionConfigurationBuilder<TOutput, TException>
    {
        private List<Action<Exception, TOutput>> _configurationActions;

        public ExceptionConfigurationBaseBuilder()
        {
            _configurationActions = new List<Action<Exception, TOutput>>();
            ExceptionType = typeof(TException);
        }

        public IEnumerable<Action<Exception, TOutput>> Actions => _configurationActions;

        public Type ExceptionType { get; }

        public IExceptionConfigurationBuilder<TOutput, TException_1> AsType<TException_1>() where TException_1 : Exception
        {
            var convertedBuilder = this as IExceptionConfigurationBuilder<TOutput, TException_1>;

            if (convertedBuilder == null)
            {
                throw new InvalidOperationException($"This builder is for the exception type {typeof(TException).FullName}");
            }

            return convertedBuilder;                
        }

        protected void AddConfiguration(Action<TException, TOutput> configurationAction)
        {
            _configurationActions.Add((exception, output) =>
            {
                if (configurationAction != null
                    && exception is TException exceptionAsType)
                {
                    configurationAction(exceptionAsType, output);
                }
            });
        }
    }
}
