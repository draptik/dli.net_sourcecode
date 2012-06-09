using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class SqlProductRepositoryTest
    {
        [Theory, AutoMoqData]
        public void SutIsProductRepository(SqlProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ProductRepository>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ConnectionStringIsCorrect([Frozen]string expectedConnectionString, SqlProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            string result = sut.ConnectionString;
            // Verify outcome
            Assert.Equal(expectedConnectionString, result);
            // Teardown
        }
    }
}
