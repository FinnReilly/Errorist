using Errorist.Models;

namespace Errorist.Implementations
{
    public class ExceptionFormattingGlobalScope<TOutput> : IExceptionFormattingGlobalScope<TOutput>
        where TOutput : class
    {
        private readonly IConfigurationBuilderFactory _configurationBuilderFactory;
        private ScopedExceptionConfigurationCollection<TOutput> _configurationCollection;

        public ExceptionFormattingGlobalScope(IConfigurationBuilderFactory configurationBuilderFactory)
        {
            _configurationBuilderFactory = configurationBuilderFactory;
            _configurationCollection = new ScopedExceptionConfigurationCollection<TOutput>();
        }

        public ExceptionScopeConfiguration<TOutput> Configuration => _configurationCollection.ResolvedConfiguration;

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

        public TBuilder ConfigureDefaultWithBuilder<TBuilder>() where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, Exception, TBuilder>
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
    }
}
