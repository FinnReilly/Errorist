using Errorist.Implementations;
using Microsoft.AspNetCore.Http;

namespace Errorist.Test.Implementations
{
    [TestFixture]
    public class DefaultContextConfiguratorTests
    {
        [Test]
        public async Task ConfigureContextWithErrorResponse_WhenOutputTypeHasStatusCode_ResponseStatusCodeAndJsonContentAreUpdated()
        {
            // Arrange
            var output = new TestOutputTypeWithStatusCode
            {
                StatusCode = 400,
                ConfigurableMessage = "Validation Error"
            };
            using var memStream = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Response.Body = memStream;
            var expectedOutput = "{\"StatusCode\":400,\"ConfigurableMessage\":\"Validation Error\"}";
            var sut = new DefaultContextConfigurator<TestOutputTypeWithStatusCode>();

            // Act
            await sut.ConfigureContextWithErrorResponse(context, output);
            context.Response.Body.Position = 0;
            var contentAsString = new StreamReader(context.Response.Body).ReadToEnd();

            // Assert
            Assert.That(context.Response.StatusCode, Is.EqualTo(400));
            Assert.That(context.Response.ContentType, Is.EqualTo("application/json"));
            Assert.That(contentAsString, Is.EqualTo(expectedOutput));
        }
        
        [Test]
        public async Task ConfigureContextWithErrorResponse_WhenOutputTypeDoesNotHaveStatusCode_JsonContentIsUpdated()
        {
            // Arrange
            var output = new TestOutputType
            {
                ConfigurableMessage = "Validation Error"
            };
            var expectedStatusCode = 500;
            using var memStream = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Response.StatusCode = 500;
            context.Response.Body = memStream;
            var expectedOutput = "{\"ConfigurableMessage\":\"Validation Error\"}";
            var sut = new DefaultContextConfigurator<TestOutputType>();

            // Act
            await sut.ConfigureContextWithErrorResponse(context, output);
            context.Response.Body.Position = 0;
            var contentAsString = new StreamReader(context.Response.Body).ReadToEnd();

            // Assert
            Assert.That(context.Response.StatusCode, Is.EqualTo(expectedStatusCode));
            Assert.That(context.Response.ContentType, Is.EqualTo("application/json"));
            Assert.That(contentAsString, Is.EqualTo(expectedOutput));
        }
    }
}
