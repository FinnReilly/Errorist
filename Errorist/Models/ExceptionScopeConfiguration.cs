namespace Errorist.Models
{
    public class ExceptionScopeConfiguration<TOutput>
        where TOutput : class
    {
        private ExceptionScopeConfiguration()
        {
            DefaultActions = new List<Action<Exception, TOutput>>();
            SpecificActions = new Dictionary<Type, List<Action<Exception, TOutput>>>();
            IsGlobal = true;
        }

        private ExceptionScopeConfiguration(Exception triggeringException)
        {
            DefaultActions = new List<Action<Exception, TOutput>>();
            SpecificActions = new Dictionary<Type, List<Action<Exception, TOutput>>>();
            TriggeringException = triggeringException;
        }

        public bool IsGlobal { get; }
        public Exception? TriggeringException { get; }
        public List<Action<Exception, TOutput>> DefaultActions { get; }
        public Dictionary<Type, List<Action<Exception, TOutput>>> SpecificActions { get; }

        public static ExceptionScopeConfiguration<TOutput> AsGlobal() => new ExceptionScopeConfiguration<TOutput>();
        public static ExceptionScopeConfiguration<TOutput> FromException(Exception exception) => new ExceptionScopeConfiguration<TOutput>(exception);
    }
}
