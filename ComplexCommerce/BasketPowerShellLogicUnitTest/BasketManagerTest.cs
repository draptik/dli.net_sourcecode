using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison.Fluent;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Commerce.BasketPowerShellModel.UnitTest
{
    public class BasketManagerTest
    {
        [Theory, AutoMoqData]
        public void GetAllBasketsWillReturnInstance(BasketManager sut)
        {
            // Fixture setup
            // Exercise system
            IEnumerable<BasketView> result = sut.GetAllBaskets();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void GetAllBasketsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var baskets = fixture.CreateMany<Basket>().ToList();
            var expectedViews = (from b in baskets
                                 select new BasketView(b).AsSource().OfLikeness<BasketView>()).ToList();

            var basketServiceStub = fixture.Freeze<Mock<IBasketService>>();
            basketServiceStub.Setup(bs => bs.GetAllBaskets()).Returns(baskets);

            var sut = fixture.CreateAnonymous<BasketManager>();
            // Exercise system
            var result = sut.GetAllBaskets();
            // Verify outcome
            Assert.True(expectedViews.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RemoveBasketForNullOwnerWillThrow(BasketManager sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.RemoveBasket(null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RemoveBasketWillRemoveBasketFromService([Frozen]Mock<IBasketService> basketServiceMock, string owner, BasketManager sut)
        {
            // Fixture setup
            // Exercise system
            sut.RemoveBasket(owner);
            // Verify outcome
            basketServiceMock.Verify(bs => bs.Empty(It.Is<IPrincipal>(p => p.Identity.Name == owner)));
            // Teardown
        }
    }
}
