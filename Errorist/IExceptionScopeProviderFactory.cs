namespace Errorist
{
    public interface IExceptionScopeProviderFactory<TOutput>
        where TOutput : class, new()
    {
        IExceptionScopeProvider<TOutput> CurrentProvider { get; }
    }
}
