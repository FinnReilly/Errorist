using Errorist.Implementations;
using Errorist.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errorist.Test.Implementations
{
    [TestFixture]
    public class ExceptionFormattingLocalScopeTests
    {
        private Mock<IScopedConfigurationQueue<TestOutputType>> _configurations;
        private Mock<IConfigurationBuilderFactory> _configurationBuilderFactory;

        private Func<ExceptionFormattingLocalScope<TestOutputType>> _sutFactory;
        private ExceptionScopeConfiguration<TestOutputType>? _stagedConfiguration;

        [SetUp]
        public void Setup()
        {
            _configurations = new Mock<IScopedConfigurationQueue<TestOutputType>>();
            _configurationBuilderFactory = new Mock<IConfigurationBuilderFactory>();    
            
            _sutFactory = () => new ExceptionFormattingLocalScope<TestOutputType>(
                _configurations.Object,
                _configurationBuilderFactory.Object);

            _configurationBuilderFactory.Setup(f => f.Create<TestBuilder<Exception>, TestOutputType, Exception>())
                .Returns(new TestBuilder<Exception>());
            _configurationBuilderFactory.Setup(f => f.Create<TestBuilder<ArgumentException>, TestOutputType, ArgumentException>())
                .Returns(new TestBuilder<ArgumentException>());
            _configurations.Setup(c => c.Enqueue(It.IsAny<ExceptionScopeConfiguration<TestOutputType>>()))
                .Callback<ExceptionScopeConfiguration<TestOutputType>>(config => _stagedConfiguration = config);
        }

        [Test]
        public void Dispose_WhenConfigurationsAddedAndNoErrorHasOccurred_NothingStagedToConfigurationQueue()
        {
            // Arrange
            using (var sut = _sutFactory())
            {
                sut.ConfigureDefault()
                    .AddConfiguration((e, dto) => dto.ConfigurableMessage = "Hello");
            // Act
            }

            // Assert
            Assert.That(_stagedConfiguration, Is.Null);
        }

        [Test]
        public void Dispose_WhenNoConfigurationsAddedAndErrorHasOccurred_EmptyConfigurationAddedToQueue()
        {
            // Arrange
            try
            {
                using (var sut = _sutFactory())
                {
                    throw new Exception();

            // Act
                }
            }
            catch
            {
            }

            // Assert
            Assert.That(_stagedConfiguration, Is.Not.Null);
            Assert.That(_stagedConfiguration.DefaultActions.Count, Is.EqualTo(0));
            Assert.That(_stagedConfiguration.SpecificActions.Count, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_WhenConfigurationsAddedAndExceptionThrown_ConfigurationIsStagedWhichSeparatesDefaultAndSpecificActionsAndListsInOrderAdded()
        {
            // Arrange
            var expectedDefaultConfigurationResult = "Default 1 Default 2";
            var expectedSpecificConfigurationResult = "Specific 1 Test Error"; 
            var testDefaultOutput = new TestOutputType();
            var testSpecificOutput = new TestOutputType();
            try
            {
                using (var sut = _sutFactory())
                {
                    sut.ConfigureDefault()
                        .AddConfiguration((e, dto) => dto.ConfigurableMessage = "Default 1");
                    sut.ConfigureDefaultWithBuilder<TestBuilder<Exception>>()
                        .AddConfiguration((e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " Default 2");
                    sut.Configure<ArgumentException>()
                        .AddConfiguration((e, dto) => dto.ConfigurableMessage = "Specific 1");
                    sut.Configure<KeyNotFoundException>()
                        .AddConfiguration((e, dto) => dto.ConfigurableMessage = "Not used in this test");
                    sut.ConfigureWithBuilder<TestBuilder<ArgumentException>, ArgumentException>()
                        .AddConfiguration((e, dto) => dto.ConfigurableMessage = dto.ConfigurableMessage + " " + e.Message);
            
                    throw new ArgumentException("Test Error");
            // Act
                }
            }
            catch (Exception e)
            {
                foreach (var action in _stagedConfiguration.DefaultActions)
                {
                    action(e, testDefaultOutput);
                }

                foreach (var action in _stagedConfiguration.SpecificActions.SelectMany(kvp => kvp.Value))
                {
                    action(e, testSpecificOutput);
                }
            }

            // Assert
            Assert.That(_stagedConfiguration, Is.Not.Null);
            Assert.That(_stagedConfiguration.DefaultActions.Count, Is.EqualTo(2));
            Assert.That(_stagedConfiguration.SpecificActions.Count, Is.EqualTo(2));
            Assert.That(_stagedConfiguration.SpecificActions[typeof(ArgumentException)].Count, Is.EqualTo(2));
            Assert.That(testDefaultOutput.ConfigurableMessage, Is.EqualTo(expectedDefaultConfigurationResult));
            Assert.That(testSpecificOutput.ConfigurableMessage, Is.EqualTo(expectedSpecificConfigurationResult)); 
        }

        [Test]
        public void ConfigureDefault_ReturnsGenericBuilder()
        {
            // Arrange
            using var sut = _sutFactory();

            // Act
            var builder = sut.ConfigureDefault();

            // Assert
            Assert.True(builder is GenericExceptionConfigurationBuilder<TestOutputType, Exception>);
        }

        [Test]
        public void Configure_ReturnsGenericBuilderForCorrectType()
        {
            // Arrange
            using var sut = _sutFactory();

            // Act
            var builder = sut.Configure<ArgumentException>();

            // Assert
            Assert.True(builder is GenericExceptionConfigurationBuilder<TestOutputType, ArgumentException>);
        }

        [Test]
        public void ConfigureDefaultWithBuilder_ReturnsSpecifiedBuilderType()
        {
            // Arrange
            using var sut = _sutFactory();

            // Act
            var builder = sut.ConfigureDefaultWithBuilder<TestBuilder<Exception>>();

            // Assert
            Assert.True(builder is TestBuilder<Exception>);
            _configurationBuilderFactory.Verify(
                f => f.Create<TestBuilder<Exception>, TestOutputType, Exception>(),
                Times.Once);
        }

        [Test]
        public void ConfigureWithBuilder_ReturnsSpecifiedBuilderType()
        {
            // Arrange
            using var sut = _sutFactory();

            // Act
            var builder = sut.ConfigureWithBuilder<TestBuilder<ArgumentException>, ArgumentException>();

            // Assert
            Assert.True(builder is TestBuilder<ArgumentException>);
            _configurationBuilderFactory.Verify(
                f => f.Create<TestBuilder<ArgumentException>, TestOutputType, ArgumentException>(),
                Times.Once);
        }

        [TearDown]
        public void TearDown()
        {
            _stagedConfiguration = null;
        }
    }
}
