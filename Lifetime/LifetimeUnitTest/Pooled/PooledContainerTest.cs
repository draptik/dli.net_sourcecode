using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime.Pooled;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest.Pooled
{
    public class PooledContainerTest
    {
        [Theory, AutoMoqData]
        public void SutIsCommerceServiceContainer(PooledContainer sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ICommerceServiceContainer>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveWillReturnCorrectType(PooledContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            Assert.IsAssignableFrom<ProductManagementService>(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveTwiceWillReturnDifferentInstances(PooledContainer sut)
        {
            // Fixture setup
            sut.MaxSize = 10;
            var unexpected = sut.ResolveProductManagementService();
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            Assert.NotSame(unexpected, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveWillReturnResultWithCorrectRepository(PooledContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            var actual = Assert.IsAssignableFrom<ProductManagementService>(result);
            Assert.IsAssignableFrom<XferProductRepository>(actual.Repository);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveTwiceWillReturnResultsWithDifferentRepositories(PooledContainer sut)
        {
            // Fixture setup
            sut.MaxSize = 2;
            var other = sut.ResolveProductManagementService();
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            var srvc1 = Assert.IsAssignableFrom<ProductManagementService>(other);
            var srvc2 = Assert.IsAssignableFrom<ProductManagementService>(result);
            Assert.NotSame(srvc1.Repository, srvc2.Repository);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolveOverMaxSizeWillThrow(PooledContainer sut)
        {
            // Fixture setup
            sut.MaxSize = 1;
            sut.ResolveProductManagementService();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() => sut.ResolveProductManagementService());
            // Teardown
        }

        [Fact]
        public void ResolveAfterReleaseWillReturnResultWithRepositoryFromPool()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<PooledContainer>();
            sut.MaxSize = fixture.RepeatCount;

            var services = fixture.Repeat(() => sut.ResolveProductManagementService()).ToList();
            var observedRepositories = from s in services
                                       select ((ProductManagementService)s).Repository;
            services.ForEach(sut.Release);
            // Exercise system
            var result = sut.ResolveProductManagementService();
            // Verify outcome
            var actual = Assert.IsAssignableFrom<ProductManagementService>(result);
            Assert.Contains(actual.Repository, observedRepositories);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RepeatedlyResolvingAndRemovingMustBePossible(PooledContainer sut)
        {
            // Fixture setup
            sut.MaxSize = 1;

            // Exercise system
            Assert.DoesNotThrow(() =>
            {
                sut.Release(sut.ResolveProductManagementService());
                sut.Release(sut.ResolveProductManagementService());
                sut.Release(sut.ResolveProductManagementService());
            });
            // Verify outcome
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ResolvingServicesAndReleasingItAndThenResolvingTwiceShouldThrow(PooledContainer sut)
        {
            // Fixture setup
            sut.MaxSize = 1;

            var s = sut.ResolveProductManagementService();
            sut.Release(s);
            sut.ResolveProductManagementService();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() => sut.ResolveProductManagementService());
            // Teardown
        }
    }
}
