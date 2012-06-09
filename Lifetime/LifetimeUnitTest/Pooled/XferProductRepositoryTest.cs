using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest.Pooled
{
    public class XferProductRepositoryTest
    {
        [Theory, AutoMoqData]
        public void SutIsProductRepository(XferProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ProductRepository>(sut);
            // Teardown
        }
    }
}
