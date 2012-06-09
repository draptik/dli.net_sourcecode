using System;
using Moq;
using Moq.Protected;
using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime.Disposable.DisposableConsumer;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest.Disposable.DisposableConsumer
{
    public class OrderServiceTest
    {
        [Theory, AutoMoqData]
        public void SutIsDisposable(OrderService sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IDisposable>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RepositoryIsCorrect([Frozen]OrderRepository expectedRepository, OrderService sut)
        {
            // Fixture setup
            // Exercise system
            OrderRepository result = sut.Repository;
            // Verify outcome
            Assert.Equal(expectedRepository, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DisposeWillDisposeRepository([Frozen]Mock<OrderRepository> repositoryMock, OrderService sut)
        {
            // Fixture setup
            repositoryMock.Protected().Setup("Dispose", true).Verifiable();
            // Exercise system
            sut.Dispose();
            // Verify outcome
            repositoryMock.Verify();
            // Teardown
        }
    }
}
