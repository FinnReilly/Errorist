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
    public class ScopedConfigurationQueueTests
    {
        private Mock<IExceptionFormattingGlobalScope<TestOutputType>> _globalScope;
        private ExceptionScopeConfiguration<TestOutputType> _globalConfiguration;

        private ScopedConfigurationQueue<TestOutputType> _sut;

        [SetUp]
        public void Setup()
        {
            _globalConfiguration = new ExceptionScopeConfiguration<TestOutputType>();
            _globalScope = new Mock<IExceptionFormattingGlobalScope<TestOutputType>>();
            _globalScope.Setup(scope => scope.Configuration).Returns(_globalConfiguration).Verifiable();

            _sut = new ScopedConfigurationQueue<TestOutputType>(_globalScope.Object);
        }

        [Test]
        public void Enqueue_IncreasesCount()
        {
            // Arrange
            var enqueued_1 = new ExceptionScopeConfiguration<TestOutputType>();
            var enqueued_2 = new ExceptionScopeConfiguration<TestOutputType>();
            var initialCount = _sut.Count;

            // Act
            _sut.Enqueue(enqueued_1);
            var countAfterFirstEnqueue = _sut.Count;
            _sut.Enqueue(enqueued_2);
            var countAfterSecondEnqueue = _sut.Count;

            // Assert
            Assert.That(initialCount, Is.EqualTo(1));
            Assert.That(countAfterFirstEnqueue, Is.EqualTo(2));
            Assert.That(countAfterSecondEnqueue, Is.EqualTo(3));
        }

        [Test]
        public void Dequeue_WhenItemsInQueue_DecrementsCountAndReturnsLastInputLast()
        {
            // Arrange
            var enqueued_1 = new ExceptionScopeConfiguration<TestOutputType>();
            var enqueued_2 = new ExceptionScopeConfiguration<TestOutputType>();
            _sut.Enqueue(enqueued_1);
            _sut.Enqueue(enqueued_2);
            var countAfterEnqueues = _sut.Count;

            // Act
            var dequeued_1 = _sut.Dequeue();
            var countAfterFirstDequeue = _sut.Count;
            var dequeued_2 = _sut.Dequeue();
            var countAfterSecondDequeue = _sut.Count;
            var dequeued_3 = _sut.Dequeue();
            var countAfterThirdDequeue = _sut.Count;

            // Assert
            Assert.That(countAfterEnqueues, Is.EqualTo(3));
            Assert.That(countAfterFirstDequeue, Is.EqualTo(2));
            Assert.That(countAfterSecondDequeue, Is.EqualTo(1));
            Assert.That(countAfterThirdDequeue, Is.EqualTo(0));
            Assert.That(dequeued_1, Is.EqualTo(_globalConfiguration));
            Assert.That(dequeued_2, Is.EqualTo(enqueued_1));
            Assert.That(dequeued_3, Is.EqualTo(enqueued_2));
        }

        [Test]
        public void Dequeue_WhenQueueEmpty_Throws()
        {
            // Arrange
            _sut.Dequeue();

            // Act/Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _sut.Dequeue());
        }
    }
}
