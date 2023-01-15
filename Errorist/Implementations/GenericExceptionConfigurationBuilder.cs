using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errorist.Implementations
{
    public class GenericExceptionConfigurationBuilder<TOutput, TException> : ExceptionConfigurationBaseBuilder<TOutput, TException, GenericExceptionConfigurationBuilder<TOutput, TException>>
        where TException : Exception
        where TOutput : class
    {
    }
}
