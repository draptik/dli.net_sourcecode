using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest.Singleton
{
    public class InMemoryProductRepositoryTest
    {
        [Theory, AutoMoqData]
        public void SutIsProductRepository(InMemoryProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ProductRepository>(sut);
            // Teardown
        }
    }
}
