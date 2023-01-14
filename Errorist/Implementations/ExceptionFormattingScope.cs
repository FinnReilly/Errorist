using Errorist.Models;

namespace Errorist.Implementations
{
    public class ExceptionFormattingScope<TOutput, TBuilder> : IExceptionFormattingScope<TOutput>
        where TOutput : class
        where TBuilder : IExceptionConfigurationBuilder<TOutput>, new()
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

        public IExceptionConfigurationBuilder<TOutput, TException> Configure<TException>() where TException : Exception
        {
            var builder = new TBuilder().AsType<TException>();
            _builders.Add(builder);
            return builder;
        }

        public IExceptionConfigurationBuilder<TOutput, Exception> ConfigureDefault()
        {
            var builder = new TBuilder().AsType<Exception>();
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
