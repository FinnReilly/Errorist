namespace Errorist
{
    public interface IExceptionFormattingService<TOutput>
        where TOutput : class
    {
        IExceptionFormattingScope<TOutput> GetScope();
        TOutput Configure(TOutput output, Exception exception);
    }
}
