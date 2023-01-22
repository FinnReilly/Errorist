namespace Errorist
{
    public interface IExceptionFormattingService<TOutput>
        where TOutput : class, new()
    {
        IExceptionFormattingLocalScope<TOutput> GetScope();
        TOutput Configure(TOutput output, Exception exception);
    }
}
