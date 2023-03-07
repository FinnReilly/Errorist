using Errorist.Implementations;
using Errorist.Models;
using Moq;

namespace Errorist.Test.Implementations
{
    [TestFixture]
    public class ExceptionOutputConfiguratorTests
    {
        private Mock<IScopedConfigurationQueue<TestOutputType>> _configurations;

        private ExceptionOutputConfigurator<TestOutputType> _sut;

        private TestOutputType _input;
        private Queue<ExceptionScopeConfiguration<TestOutputType>> _outputQueue;

        [SetUp]
        public void Setup()
        {
            _configurations = new Mock<IScopedConfigurationQueue<TestOutputType>>();

            _sut = new ExceptionOutputConfigurator<TestOutputType>(_configurations.Object);

            _input = new TestOutputType();
            _outputQueue = new Queue<ExceptionScopeConfiguration<TestOutputType>>();
            _configurations.Setup(c => c.Dequeue()).Returns(() => _outputQueue.Dequeue());
            _configurations.Setup(c => c.Count).Returns(() => _outputQueue.Count);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                new ArgumentNullException(),
                "An exception was thrown in a service (ArgumentNullException) in a controller. The argument was null.");
            yield return new TestCaseData(
                new InvalidOperationException(),
                "An InvalidOperationException was thrown in a service in a controller. The operation was invalid.");
        }


        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void Configure_AppliesConfigurationsInExpectedOrder(Exception exception, string expectedModelPropertyValue)
        {
            // Arrange
            var globalConfig = new ExceptionScopeConfiguration<TestOutputType>();
            var serviceLevelConfig = new ExceptionScopeConfiguration<TestOutputType>();
            var controllerLevelConfig = new ExceptionScopeConfiguration<TestOutputType>();
            globalConfig.DefaultActions.Add((e, dto) => dto.ConfigurableMessage = "An exception was thrown");
            globalConfig.SpecificActions.Add(
                typeof(InvalidOperationException),
                new List<Action<Exception, TestOutputType>> { (e, dto) => dto.ConfigurableMessage = "An InvalidOperationException was thrown" });
            serviceLevelConfig.DefaultActions.Add((e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " in a service");
            serviceLevelConfig.SpecificActions.Add(
                typeof(ArgumentNullException),
                new List<Action<Exception, TestOutputType>> { (e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " (ArgumentNullException)" });
            controllerLevelConfig.DefaultActions.Add((e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " in a controller.");
            controllerLevelConfig.SpecificActions.Add(
                typeof(InvalidOperationException),
                new List<Action<Exception, TestOutputType>> { (e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " The operation was invalid." });
            controllerLevelConfig.SpecificActions.Add(
                typeof(ArgumentNullException),
                new List<Action<Exception, TestOutputType>> { (e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " The argument was null."});

            _outputQueue.Enqueue(globalConfig);
            _outputQueue.Enqueue(serviceLevelConfig);
            _outputQueue.Enqueue(controllerLevelConfig);

            // Act
            var output = _sut.Configure(new TestOutputType(), exception);

            // Assert
            Assert.That(output.ConfigurableMessage, Is.EqualTo(expectedModelPropertyValue));
        }
    }
}
