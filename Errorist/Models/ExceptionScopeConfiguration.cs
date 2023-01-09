namespace Errorist.Models
{
    public class ExceptionScopeConfiguration<TOutput>
        where TOutput : class
    {
        public ExceptionScopeConfiguration()
        {
            DefaultActions = new List<Action<Exception, TOutput>>();
            SpecificActions = new Dictionary<Type, List<Action<Exception, TOutput>>>();
        }

        public List<Action<Exception, TOutput>> DefaultActions { get; }
        public Dictionary<Type, List<Action<Exception, TOutput>>> SpecificActions { get; }
    }
}
