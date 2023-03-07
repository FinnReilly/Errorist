using Errorist.Implementations;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Errorist.Test.Implementations
{
    [TestFixture]
    public class ExceptionScopeProviderFactoryTests
    {
        private Mock<IHttpContextAccessor> _contextAccessor;

        private ExceptionScopeProviderFactory<TestOutputType> _sut;

        private DefaultHttpContext _returnedContext;
        private Mock<IServiceProvider> _requestServiceProvider;
        private Mock<IExceptionScopeProvider<TestOutputType>> _returnedScopeProvider;

        [SetUp]
        public void Setup()
        {
            _contextAccessor = new Mock<IHttpContextAccessor>();

            _sut = new ExceptionScopeProviderFactory<TestOutputType>(_contextAccessor.Object);

            _requestServiceProvider = new Mock<IServiceProvider>();
            _returnedScopeProvider = new Mock<IExceptionScopeProvider<TestOutputType>>();
            _returnedContext = new DefaultHttpContext 
            {
                RequestServices = _requestServiceProvider.Object,
            };

            _requestServiceProvider
                .Setup(s => s.GetService(typeof(IExceptionScopeProvider<TestOutputType>)))
                .Returns(_returnedScopeProvider.Object)
                .Verifiable();
            _contextAccessor.Setup(s => s.HttpContext)
                .Returns(_returnedContext)
                .Verifiable();
        }

        [Test]
        public void CurrentProvider_ReturnsScopedInstanceFromRequestServices()
        {
            // Act
            var actualScopeProvider = _sut.CurrentProvider;

            // Assert
            _contextAccessor.Verify();
            _requestServiceProvider.Verify();
            Assert.That(actualScopeProvider, Is.EqualTo(_returnedScopeProvider.Object));
        }
    }
}
