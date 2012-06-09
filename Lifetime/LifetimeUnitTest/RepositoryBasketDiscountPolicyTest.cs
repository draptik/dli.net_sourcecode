using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class RepositoryBasketDiscountPolicyTest
    {
        [Theory, AutoMoqData]
        public void SutIsBasketDiscountPolicy(RepositoryBasketDiscountPolicy sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<BasketDiscountPolicy>(sut);
            // Teardown
        }

        [Fact]
        public void GetDiscountedProductsWillReturnCorrectResults()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expectedProducts = fixture.CreateMany<Product>().ToList();

            fixture.Freeze<Mock<DiscountRepository>>().Setup(r => r.GetDiscountedProducts()).Returns(expectedProducts);

            var sut = fixture.CreateAnonymous<RepositoryBasketDiscountPolicy>();
            // Exercise system
            var result = sut.GetDiscountedProducts();
            // Verify outcome
            Assert.True(expectedProducts.SequenceEqual(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RepositoryIsCorrect([Frozen]DiscountRepository expectedRepository, RepositoryBasketDiscountPolicy sut)
        {
            // Fixture setup
            // Exercise system
            DiscountRepository result = sut.Repository;
            // Verify outcome
            Assert.Equal(expectedRepository, result);
            // Teardown
        }
    }
}
