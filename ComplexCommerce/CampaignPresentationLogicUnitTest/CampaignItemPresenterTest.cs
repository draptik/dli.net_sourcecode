using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Commerce.CampaignPresentation.UnitTest
{
    public class CampaignItemPresenterTest
    {
        [Theory, AutoMoqData]
        public void IdIsProperWritableProperty([Frozen]int expectedId, CampaignItemPresenter sut)
        {
            // Fixture setup
            // Exercise system
            sut.Id = expectedId;
            int result = sut.Id;
            // Verify outcome
            Assert.Equal(expectedId, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ProductNameIsProperWritableProperty([Frozen]string expectedName, CampaignItemPresenter sut)
        {
            // Fixture setup
            // Exercise system
            sut.ProductName = expectedName;
            string result = sut.ProductName;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UnitPriceIsProperWritableProperty([Frozen]decimal expectedPrice, CampaignItemPresenter sut)
        {
            // Fixture setup
            // Exercise system
            sut.UnitPrice = expectedPrice;
            decimal? result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal(expectedPrice, result);
            // Teardown
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsFeaturedIsProperWritableProperty(bool expectedResult)
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<CampaignItemPresenter>();
            // Exercise system
            sut.IsFeatured = expectedResult;
            bool result = sut.IsFeatured;
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DiscountPriceIsProperWritableProperty(decimal expectedResult, CampaignItemPresenter sut)
        {
            // Fixture setup
            // Exercise system
            sut.DiscountPrice = expectedResult;
            decimal? result = sut.DiscountPrice;
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
