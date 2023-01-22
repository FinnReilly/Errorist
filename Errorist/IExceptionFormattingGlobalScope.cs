using Errorist.Models;

namespace Errorist
{
    public interface IExceptionFormattingGlobalScope<TOutput> : IExceptionFormattingScope<TOutput>
        where TOutput : class
    {
        public ExceptionScopeConfiguration<TOutput> Configuration { get; }
    }
}
