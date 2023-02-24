namespace Errorist.Implementations
{
    public class ExceptionOutputConfigurator<TOutput> : IExceptionOutputConfigurator<TOutput>
        where TOutput : class, new()
    {
        private readonly IScopedConfigurationQueue<TOutput> _configurations;

        public ExceptionOutputConfigurator(IScopedConfigurationQueue<TOutput> configurations)
        {
            _configurations = configurations;
        }

        public TOutput Configure(TOutput output, Exception exception)
        {
            var exceptionType = exception.GetType();

            return this.ApplyConfigurations(output, exception, exceptionType);
        }

        private TOutput ApplyConfigurations(TOutput output, Exception e, Type t)
        {
            while (_configurations.Count > 0)
            {
                var config = _configurations.Dequeue();
                foreach (var action in config.DefaultActions)
                {
                    action.Invoke(e, output);
                }

                if (config.SpecificActions.TryGetValue(t, out var actions))
                {
                    foreach (var action in actions)
                    {
                        action.Invoke(e, output);
                    }
                }
            }

            return output;
        }
    }
}
