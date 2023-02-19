namespace Errorist
{
    public interface IExceptionOutputConfigurator<TOutput>
        where TOutput : class, new()
    {
        TOutput Configure(TOutput output, Exception exception);
    }
}
