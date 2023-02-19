namespace Errorist
{
    public interface IExceptionScopeProvider<TOutput>
        where TOutput : class, new()
    {
        IExceptionFormattingLocalScope<TOutput> GetScope();
    }
}
