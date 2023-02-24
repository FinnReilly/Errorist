using Errorist.Implementations;

namespace Errorist.Test.Implementations
{
    [TestFixture]
    public class ConfigurationBuilderFactoryTests
    {
        private ConfigurationBuilderFactory _sut;

        public class TestBuilderTakesParameters<TException> : ExceptionConfigurationBaseBuilder<TestOutputType, TException, TestBuilderTakesParameters<TException>>
            where TException : Exception
        {
            public TestBuilderTakesParameters(int someArgumentOrOther)
            {
            }
        }

        [SetUp]
        public void Setup()
        {
            _sut = new ConfigurationBuilderFactory();
        }

        [Test]
        public void Create_WhenAttemptingToCreateTypeWithDefaultParameterlessConstructor_CanCreateBuilder()
        {
            // Act
            var newBuilder = _sut.Create<TestBuilder<ArgumentException>, TestOutputType, ArgumentException>();

            // Assert
            Assert.NotNull(newBuilder);
            Assert.That(newBuilder.GetType(), Is.EqualTo(typeof(TestBuilder<ArgumentException>)));
        }

        [Test]
        public void Create_WhenAttemptingToCreateTypeWithoutParameterlessConstructor_ThrowsExplanatoryError()
        {
            // Arrange
            var expectedErrorMessage = "The default IConfigurationBuilderFactory cannot handle builder of type " +
                "Errorist.Test.Implementations.ConfigurationBuilderFactoryTests+TestBuilderTakesParameters`1[[System.Collections.Generic.KeyNotFoundException, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]. " +
                "If you want to use this builder type you will need to register an IConfigurationBuilderFactory that is capable of instantiating this class.";

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _sut.Create<TestBuilderTakesParameters<KeyNotFoundException>, TestOutputType, KeyNotFoundException>());

            // Assert
            Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
        }
    }
}
