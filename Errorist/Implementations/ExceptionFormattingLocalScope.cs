using Errorist.Models;

namespace Errorist.Implementations
{
    public class ExceptionFormattingLocalScope<TOutput> : IExceptionFormattingLocalScope<TOutput>
        where TOutput : class, new()
    {
        private readonly IScopedConfigurationQueue<TOutput> _configurations;
        private readonly IConfigurationBuilderFactory _configurationBuilderFactory;
        private readonly ScopedExceptionConfigurationCollection<TOutput> _configurationCollection;
        private bool _scopeComplete = true;

        public ExceptionFormattingLocalScope(
            IScopedConfigurationQueue<TOutput> configurations,
            IConfigurationBuilderFactory configurationBuilderFactory)
        {
            _configurationBuilderFactory = configurationBuilderFactory;
            _configurations = configurations;
            _configurationCollection = new ScopedExceptionConfigurationCollection<TOutput>();
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }

        public GenericExceptionConfigurationBuilder<TOutput, TException> Configure<TException>() where TException : Exception
        {
            var builder = new GenericExceptionConfigurationBuilder<TOutput, TException>();
            _configurationCollection.AddBuilder(builder);
            return builder;
        }

        public GenericExceptionConfigurationBuilder<TOutput, Exception> ConfigureDefault()
        {
            var builder = new GenericExceptionConfigurationBuilder<TOutput, Exception>();
            _configurationCollection.AddDefaultBuilder(builder);
            return builder;
        }

        public TBuilder ConfigureDefaultWithBuilder<TBuilder>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, Exception, TBuilder>
        {
            var builder = _configurationBuilderFactory.Create<TBuilder, TOutput, Exception>();
            _configurationCollection.AddDefaultBuilder(builder);
            return builder;
        }

        public TBuilder ConfigureWithBuilder<TBuilder, TException>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>
            where TException : Exception
        {
            var builder = _configurationBuilderFactory.Create<TBuilder, TOutput, TException>();
            _configurationCollection.AddBuilder(builder);
            return builder;
        }

        public void Dispose()
        {
            if (!_scopeComplete)
            {
                _configurations.Enqueue(_configurationCollection.ResolvedConfiguration);
            }
        }

        private void CurrentDomain_FirstChanceException(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
            => _scopeComplete = false;
    }
}
