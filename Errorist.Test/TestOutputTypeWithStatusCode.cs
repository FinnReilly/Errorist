using Errorist.Models;

namespace Errorist.Test
{
    public class TestOutputTypeWithStatusCode : TestOutputType, IHasStatusCode
    {
        public int StatusCode { get; set; }
    }
}
