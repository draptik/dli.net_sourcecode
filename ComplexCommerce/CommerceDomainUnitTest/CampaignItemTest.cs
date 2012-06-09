using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class CampaignItemTest
    {
        [Theory, AutoMoqData]
        public void ProductIsCorrect([Frozen]Product expectedProduct, CampaignItem sut)
        {
            // Fixture setup
            // Exercise system
            Product result = sut.Product;
            // Verify outcome
            Assert.Equal(expectedProduct, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void IsFeaturedIsCorrect([Frozen]bool expectedResult, CampaignItem sut)
        {
            // Fixture setup
            // Exercise system
            bool result = sut.IsFeatured;
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DiscountPriceIsCorrect([Frozen]Money expectedPrice, CampaignItem sut)
        {
            // Fixture setup
            // Exercise system
            Money result = sut.DiscountPrice;
            // Verify outcome
            Assert.Equal(expectedPrice, result);
            // Teardown
        }

        [Fact]
        public void DiscountPriceCanBeNull()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject((Money)null);
            var sut = fixture.CreateAnonymous<CampaignItem>();
            // Exercise system
            var result = sut.DiscountPrice;
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }
    }
}
