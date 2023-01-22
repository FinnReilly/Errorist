using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Errorist.Implementations
{
    public class ConfigurationBuilderFactory : IConfigurationBuilderFactory
    {
        private static Dictionary<Type, bool> _parameterlessConstructorExists = new Dictionary<Type, bool>();
        private MethodInfo? _parameterlessConstructorMethodInfo = typeof(ConfigurationBuilderFactory).GetMethod(nameof(CreateWithParameterlessConstructor));
        private static Dictionary<Type, MethodInfo?> _parameterlessConstructorMethods = new Dictionary<Type, MethodInfo?>();

        public TBuilder Create<TBuilder, TOutput, TException>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>
            where TOutput : class
            where TException : Exception
        {
            var type = typeof(TBuilder);
            if (!_parameterlessConstructorExists.TryGetValue(type, out var constructorFound))
            {
                 constructorFound = type.GetConstructor(Array.Empty<Type>()) != null;
                _parameterlessConstructorExists[type] = constructorFound;
            }

            if (constructorFound)
            {
                if (!_parameterlessConstructorMethods.TryGetValue(type, out var method))
                {
                    method = _parameterlessConstructorMethodInfo?.MakeGenericMethod(type.GenericTypeArguments);
                    _parameterlessConstructorMethods[type] = method;
                }

                if (method?.Invoke((object)this, Array.Empty<object>()) is TBuilder builderResult)
                {
                    return builderResult;
                };
            }

            throw new InvalidOperationException(
                $"The default IConfigurationBuilderFactory cannot handle builder of type {type.FullName}. " +
                $"If you want to use this builder type you will need to register an IConfigurationBuilderFactory " +
                $"that is capable of instantiating this class.");
        }

        private TBuilder CreateWithParameterlessConstructor<TBuilder, TOutput, TException>()
            where TBuilder : ExceptionConfigurationBaseBuilder<TOutput, TException, TBuilder>, new()
            where TOutput : class
            where TException : Exception
            => new TBuilder();
    }
}
