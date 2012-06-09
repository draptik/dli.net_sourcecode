using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace CommerceDomainUnitTest
{
    public class DiscountedProductTest
    {
        [Fact]
        public void CreateWithNullNameWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            string nullName = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<DiscountedProduct>()
                    .FromFactory((decimal unitPrice) => new DiscountedProduct(nullName, unitPrice))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void NameWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            string expectedName = fixture.CreateAnonymous("Name");
            var sut = fixture.Build<DiscountedProduct>()
                .FromFactory((decimal unitPrice) => new DiscountedProduct(expectedName, unitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            string result = sut.Name;
            // Verify outcome
            Assert.Equal<string>(expectedName, result);
            // Teardown
        }

        [Fact]
        public void UnitPriceWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedUnitPrice = fixture.CreateAnonymous<decimal>();
            var sut = fixture.Build<DiscountedProduct>()
                .FromFactory((string name) => new DiscountedProduct(name, expectedUnitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            decimal result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<decimal>(expectedUnitPrice, result);
            // Teardown
        }
    }
}
