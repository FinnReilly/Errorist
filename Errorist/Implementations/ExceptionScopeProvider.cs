namespace Errorist.Implementations
{
    public class ExceptionScopeProvider<TOutput> : IExceptionScopeProvider<TOutput>
        where TOutput : class, new()
    {
        private readonly IScopedConfigurationQueue<TOutput> _configurations;
        private readonly IConfigurationBuilderFactory _configurationBuilderFactory;

        public ExceptionScopeProvider(
            IScopedConfigurationQueue<TOutput> configurations,
            IConfigurationBuilderFactory configurationBuilderFactory)
        {
            _configurations = configurations;
            _configurationBuilderFactory = configurationBuilderFactory;
        }

        public IExceptionFormattingLocalScope<TOutput> GetScope()
            => new ExceptionFormattingLocalScope<TOutput>(_configurations, _configurationBuilderFactory);
    }
}
