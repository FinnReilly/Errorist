namespace TestApplication.Services
{
    public interface IServiceWithTryCatch
    {
        void PerformAction(bool shouldFailAfterTryCatch);
    }
}
