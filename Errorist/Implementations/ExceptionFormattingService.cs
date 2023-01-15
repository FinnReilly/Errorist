using Errorist.Models;

namespace Errorist.Implementations
{
    public class ExceptionFormattingService<TOutput> : IExceptionFormattingService<TOutput>
        where TOutput : class
    {
        private readonly Queue<ExceptionScopeConfiguration<TOutput>> _configurations;

        public ExceptionFormattingService()
        {
            _configurations = new Queue<ExceptionScopeConfiguration<TOutput>>();
        }

        public TOutput Configure(TOutput output, Exception exception)
        {
            var exceptionType = exception.GetType();

            return this.ApplyConfigurations(output, exception, exceptionType);
        }

        public IExceptionFormattingScope<TOutput> GetScope() =>
            new ExceptionFormattingScope<TOutput>(_configurations);

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
