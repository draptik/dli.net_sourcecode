using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class BasketViewModelTest
    {
        [Fact]
        public void CreateWithNullBasketWillThrow()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            Basket nullBasket = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<BasketViewModel>()
                    .FromFactory(() => new BasketViewModel(nullBasket))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void ContentsWillBeCorrect()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            
            var basket = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(basket.Contents);

            var expectedResult = (from e in basket.Contents
                                  select string.Format("{0} {1}: {2:C}", e.Quantity, e.Product.Name, e.Total)).ToList();

            var sut = fixture.Build<BasketViewModel>()
                .FromFactory(() => new BasketViewModel(basket))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            IEnumerable<string> result = sut.Contents;
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }
    }
}
