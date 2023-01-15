using Errorist.Models;

namespace Errorist.Implementations
{
    public class ExceptionFormattingScope<TOutput> : IExceptionFormattingScope<TOutput>
        where TOutput : class
    {
        private readonly Queue<ExceptionScopeConfiguration<TOutput>> _configurations;
        private readonly List<IExceptionConfigurationBuilder<TOutput>> _builders;
        private bool _scopeComplete = false;
        private ExceptionScopeConfiguration<TOutput> _configuration;

        public ExceptionFormattingScope(Queue<ExceptionScopeConfiguration<TOutput>> configurations)
        {
            _configurations = configurations;
            _builders = new List<IExceptionConfigurationBuilder<TOutput>>();
            _configuration = new ExceptionScopeConfiguration<TOutput>();
        }

        public void Complete() => _scopeComplete = true;

        public GenericExceptionConfigurationBuilder<TOutput, TException> Configure<TException>() where TException : Exception
        {
            var builder = new GenericExceptionConfigurationBuilder<TOutput, TException>();
            _builders.Add(builder);
            return builder;
        }

        public GenericExceptionConfigurationBuilder<TOutput, Exception> ConfigureDefault()
        {
            var builder = new GenericExceptionConfigurationBuilder<TOutput, Exception>();
            _builders.Add(builder);
            return builder;
        }

        public TBuilder ConfigureDefaultWithBuilder<TBuilder>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, Exception, TBuilder>, new()
        {
            var builder = new TBuilder();
            _builders.Add(builder);
            return builder;
        }

        public TBuilder ConfigureWithBuilder<TBuilder, TException>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>, new()
            where TException : Exception
        {
            var builder = new TBuilder();
            _builders.Add(builder);
            return builder;
        }

        public void Dispose()
        {
            if (!_scopeComplete)
            {
                var defaultBuilder = _builders.FirstOrDefault(b => b.ExceptionType == typeof(object));

                if (defaultBuilder != null)
                {
                    _configuration.DefaultActions.AddRange(defaultBuilder.Actions);
                    _builders.Remove(defaultBuilder);
                }

                foreach (var builder in _builders)
                {
                    _configuration.SpecificActions.Add(
                        builder.ExceptionType,
                        builder.Actions.ToList());
                }

                _configurations.Enqueue(_configuration);
            }
        }
    }
}
