using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime.Singleton;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest.Singleton
{
    public class SingletonContainerTest
    {
        [Theory, AutoMoqData]
        public void SutIsCommerceServiceContainer(SingletonContainer sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ICommerceServiceContainer>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveWillReturnCorrectType(SingletonContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            Assert.IsAssignableFrom<ProductManagementService>(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveTwiceWillReturnDifferentInstances(SingletonContainer sut)
        {
            // Fixture setup
            var unexpected = sut.ResolveProductManagementService();
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            Assert.NotSame(unexpected, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveWillReturnResultWithCorrectRepository(SingletonContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = (ProductManagementService)sut.ResolveProductManagementService();
            // Verify outcome
            Assert.IsAssignableFrom<InMemoryProductRepository>(result.Repository);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RepositoryIsShared(SingletonContainer sut)
        {
            // Fixture setup
            var other = (ProductManagementService)sut.ResolveProductManagementService();
            // Exercise system
            var result = (ProductManagementService)sut.ResolveProductManagementService();
            // Verify outcome
            Assert.Same(other.Repository, result.Repository);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapperIsShared(SingletonContainer sut)
        {
            // Fixture setup
            var other = (ProductManagementService)sut.ResolveProductManagementService();
            // Exercise system
            var result = (ProductManagementService)sut.ResolveProductManagementService();
            // Verify outcome
            Assert.Same(other.ContractMapper, result.ContractMapper);
            // Teardown
        }
    }
}
