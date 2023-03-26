namespace Errorist.Models
{
    public class ScopedExceptionConfigurationCollection<TOutput>
        where TOutput : class
    {
        private List<IExceptionConfigurationBuilder<TOutput>> _builders;
        private List<IExceptionConfigurationBuilder<TOutput>> _defaultBuilders;
        private ExceptionScopeConfiguration<TOutput>? _exceptionScopeConfiguration;

        public ScopedExceptionConfigurationCollection()
        {
            _builders = new List<IExceptionConfigurationBuilder<TOutput>>();
            _defaultBuilders = new List<IExceptionConfigurationBuilder<TOutput>>();
            _exceptionScopeConfiguration = null;
        }

        public void AddDefaultBuilder(IExceptionConfigurationBuilder<TOutput> builder)
            => _defaultBuilders.Add(builder);

        public void AddBuilder(IExceptionConfigurationBuilder<TOutput> builder)
            => _builders.Add(builder);

        public ExceptionScopeConfiguration<TOutput> ResolveAsGlobal()
            => ResolveConstructedConfiguration(() => ExceptionScopeConfiguration<TOutput>.AsGlobal());

        public ExceptionScopeConfiguration<TOutput> ResolveOnException(Exception exception)
            => ResolveConstructedConfiguration(() => ExceptionScopeConfiguration<TOutput>.FromException(exception));

        private ExceptionScopeConfiguration<TOutput> ResolveConstructedConfiguration(Func<ExceptionScopeConfiguration<TOutput>> factoryFunc)
        {
            if (_exceptionScopeConfiguration == null)
            {
                _exceptionScopeConfiguration = factoryFunc();
                _exceptionScopeConfiguration.DefaultActions.AddRange(_defaultBuilders.SelectMany(b => b.Actions));
                foreach (var builder in _builders)
                {
                    if (!_exceptionScopeConfiguration.SpecificActions.TryGetValue(builder.ExceptionType, out var actionList))
                    {
                        actionList = new List<Action<Exception, TOutput>>();
                        _exceptionScopeConfiguration.SpecificActions.Add(
                            builder.ExceptionType,
                            actionList);
                    }
                    actionList.AddRange(builder.Actions);
                }
            }
            return _exceptionScopeConfiguration;
        }
    }
}
