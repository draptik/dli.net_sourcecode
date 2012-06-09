using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class SqlDiscountRepositoryTest
    {
        [Theory, AutoMoqData]
        public void SutIsDiscountRepository(SqlDiscountRepository sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<DiscountRepository>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ProductsIsNotNull(SqlDiscountRepository sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.Products;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void GetDiscountedProductsReturnsCorrectSequence()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedProducts = fixture.CreateMany<Product>().ToList();
            var sut = fixture.CreateAnonymous<SqlDiscountRepository>();
            expectedProducts.ForEach(sut.Products.Add);
            // Exercise system
            var result = sut.GetDiscountedProducts();
            // Verify outcome
            Assert.True(expectedProducts.SequenceEqual(result));
            // Teardown
        }
    }
}
