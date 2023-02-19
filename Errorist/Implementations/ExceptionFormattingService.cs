using Errorist.Models;

namespace Errorist.Implementations
{
    public class ExceptionFormattingService<TOutput> : IExceptionOutputConfigurator<TOutput>, IExceptionScopeProvider<TOutput>
        where TOutput : class, new()
    {
        private readonly Queue<ExceptionScopeConfiguration<TOutput>> _configurations;
        private readonly IConfigurationBuilderFactory _configurationBuilderFactory;

        public ExceptionFormattingService(
            IConfigurationBuilderFactory configurationBuilderFactory,
            IExceptionFormattingGlobalScope<TOutput> globalScope)
        {
            _configurationBuilderFactory = configurationBuilderFactory;
            _configurations = new Queue<ExceptionScopeConfiguration<TOutput>>();
            _configurations.Enqueue(globalScope.Configuration);
        }

        public TOutput Configure(TOutput output, Exception exception)
        {
            var exceptionType = exception.GetType();

            return this.ApplyConfigurations(output, exception, exceptionType);
        }

        public IExceptionFormattingLocalScope<TOutput> GetScope() =>
            new ExceptionFormattingLocalScope<TOutput>(_configurations, _configurationBuilderFactory);

        private TOutput ApplyConfigurations(TOutput output, Exception e, Type t)
        {
            while (_configurations.Any())
            {
                var config = _configurations.Dequeue();
                foreach (var action in config.DefaultActions)
                {
                    action.Invoke(e, output);
                }

                if (config.SpecificActions.TryGetValue(t, out var actions))
                {
                    foreach(var action in actions)
                    {
                        action.Invoke(e, output);
                    }
                }
            }

            return output;
        }
    }
}
