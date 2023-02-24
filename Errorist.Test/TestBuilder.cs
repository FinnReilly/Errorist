using Errorist.Implementations;

namespace Errorist.Test
{
    public class TestBuilder<TException> : ExceptionConfigurationBaseBuilder<TestOutputType, TException, TestBuilder<TException>>
        where TException : Exception
    {
    }
}
