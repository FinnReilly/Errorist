namespace Errorist
{
    public interface IExceptionFormattingLocalScope<TOutput> : IExceptionFormattingScope<TOutput>, IDisposable
        where TOutput : class
    {
        void Complete();
    }
}
