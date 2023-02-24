using Errorist.Models;

namespace Errorist
{
    public interface IScopedConfigurationQueue<TOutput>
        where TOutput : class, new()
    {
        void Enqueue(ExceptionScopeConfiguration<TOutput> configuration);
        ExceptionScopeConfiguration<TOutput> Dequeue();
        int Count { get; }
    }
}
