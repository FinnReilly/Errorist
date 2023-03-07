using Errorist.Implementations;
using Errorist.Models;

namespace Errorist.Test.Implementations
{
    [TestFixture]
    public class ApiExceptionDtoBuilderTests
    {
        private ApiExceptionDtoConfigurationBuilder<Exception> _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ApiExceptionDtoConfigurationBuilder<Exception>();
        }

        [Test]
        public void WithTitle_WhenUsingString_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                Title = "Original title"
            };
            var expectedTitle = "Replacement title";
            var exception = new Exception();

            // Act
            _sut.WithTitle(expectedTitle);
            foreach(var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.Title, Is.EqualTo(expectedTitle));
        }
        
        [Test]
        public void WithTitle_WhenUsingFactoryFunc_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                Title = "Original title"
            };
            var expectedTitle = "Exception thrown: Expected Message";
            var exception = new Exception("Expected Message");

            // Act
            _sut.WithTitle(e => $"Exception thrown: {e.Message}");
            foreach(var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.Title, Is.EqualTo(expectedTitle));
        }

        [Test]
        public void WithMessage_WhenUsingString_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                Message = "Original Message"
            };
            var expectedMessage = "Replacement message";
            var exception = new Exception();

            // Act
            _sut.WithMessage(expectedMessage);
            foreach (var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.Message, Is.EqualTo(expectedMessage));
        }
        
        [Test]
        public void WithMessage_WhenUsingFactoryFunc_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                Message = "Original Message"
            };
            var expectedMessage = "An exception of type System.Exception was thrown";
            var exception = new Exception();

            // Act
            _sut.WithMessage(e => $"An exception of type {e.GetType().FullName} was thrown");
            foreach (var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void WithUserAdvice_WhenUsingString_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                UserAdvice = "Original Message"
            };
            var expectedMessage = "Replacement message";
            var exception = new Exception();

            // Act
            _sut.WithUserAdvice(expectedMessage);
            foreach (var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.UserAdvice, Is.EqualTo(expectedMessage));
        }
        
        [Test]
        public void WithUserAdvice_WhenUsingFactoryFunc_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                UserAdvice = "Original Message"
            };
            var expectedMessage = "Advise your technical team about this System.Exception";
            var exception = new Exception();

            // Act
            _sut.WithUserAdvice(e => $"Advise your technical team about this {e.GetType().FullName}");
            foreach (var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.UserAdvice, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void WithStatusCode_WhenUsingInt_AddUpdatedConfiguration()
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                StatusCode = 500
            };
            var expectedStatusCode = 400;
            var exception = new Exception();

            // Act
            _sut.WithStatusCode(expectedStatusCode);
            foreach (var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Test]
        [TestCase(true, 404)]
        [TestCase(false, 400)]
        public void WithStatusCode_WhenUsingFactoryFunc_AddUpdatedConfiguration(bool isNotFound, int expectedStatusCode)
        {
            // Arrange
            var inputDto = new ApiExceptionDto
            {
                StatusCode = 500
            };
            var exception = new Exception(isNotFound ? "Key was not found in cache" : "bad request");

            // Act
            _sut.WithStatusCode(e => e.Message.ToLowerInvariant().Contains("not found") ? 404 : 400);
            foreach (var action in _sut.Actions)
            {
                action(exception, inputDto);
            }
            Assert.That(inputDto.StatusCode, Is.EqualTo(expectedStatusCode));
        }
    }
}
