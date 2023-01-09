namespace Errorist
{
    public interface IExceptionFormattingScope<TOutput>
        where TOutput : class
    {
        IExceptionConfigurationBuilder<TOutput, object> ConfigureDefault();
        IExceptionConfigurationBuilder<TOutput, TException> Configure<TException>()
            where TException : Exception;
        void Complete();
    }
}
