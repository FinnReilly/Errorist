using Errorist.Models;

namespace Errorist.Implementations
{
    public class ScopedConfigurationQueue<TOutput> : IScopedConfigurationQueue<TOutput>
        where TOutput : class, new()
    {
        private readonly Queue<ExceptionScopeConfiguration<TOutput>> _configurations;
        
        public ScopedConfigurationQueue(IExceptionFormattingGlobalScope<TOutput> globalScope)
        {
            _configurations = new Queue<ExceptionScopeConfiguration<TOutput>>();
            _configurations.Enqueue(globalScope.Configuration);
        }

        public int Count => _configurations.Count;

        public ExceptionScopeConfiguration<TOutput> Dequeue()
            => _configurations.Dequeue();

        public void Enqueue(ExceptionScopeConfiguration<TOutput> configuration)
            => _configurations.Enqueue(configuration);
    }
}
