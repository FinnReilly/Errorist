namespace Errorist
{
    public interface IExceptionFormattingScope<TOutput> : IDisposable
        where TOutput : class
    {
        IExceptionConfigurationBuilder<TOutput, Exception> ConfigureDefault();
        IExceptionConfigurationBuilder<TOutput, TException> Configure<TException>()
            where TException : Exception;
        void Complete();
    }
}
