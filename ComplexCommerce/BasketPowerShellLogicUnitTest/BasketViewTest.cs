using System;
using System.Linq;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.BasketPowerShellModel.UnitTest
{
    public class BasketViewTest
    {
        [Fact]
        public void OwnerIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var name = fixture.CreateAnonymous("Owner");
            fixture.Freeze<Mock<IPrincipal>>().SetupGet(p => p.Identity.Name).Returns(name);

            var basket = fixture.Freeze<Basket>();

            var sut = fixture.CreateAnonymous<BasketView>();
            // Exercise system
            string result = sut.Owner;
            // Verify outcome
            Assert.Equal(basket.Owner.Identity.Name, result);
            // Teardown
        }

        [Fact]
        public void LastUpdatedIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var basket = fixture.Freeze<Basket>();
            var sut = fixture.Build<BasketView>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            DateTimeOffset result = sut.LastUpdated;
            // Verify outcome
            var expectedResult = basket.Updated;
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void TotalIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var basket = fixture.Freeze<Basket>();
            fixture.AddManyTo(basket.Contents);
            var sut = fixture.Build<BasketView>().OmitAutoProperties().CreateAnonymous();
            // Exercise system
            decimal result = sut.Total;
            // Verify outcome
            var expectedResult = (from e in basket.Contents
                                  select e.Total.Amount).Sum();
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
