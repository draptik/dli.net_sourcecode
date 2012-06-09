using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Web.Models;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.WebUnitTest
{
    public class ProductViewModelTest
    {
        [Fact]
        public void NameIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            string expectedName = fixture.CreateAnonymous("Name");
            // Exercise system
            sut.Name = expectedName;
            string result = sut.Name;
            // Verify outcome
            Assert.Equal<string>(expectedName, result);
            // Teardown
        }

        [Fact]
        public void UnitPriceIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            decimal expectedPrice = fixture.CreateAnonymous<decimal>();
            // Exercise system
            sut.UnitPrice = expectedPrice;
            decimal result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<decimal>(expectedPrice, result);
            // Teardown
        }

        [Fact]
        public void SummaryTextWillBeCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            string expectedSummaryText = sut.Name + " (" + sut.UnitPrice.ToString("C") + ")";
            // Exercise system
            string result = sut.SummaryText;
            // Verify outcome
            Assert.Equal<string>(expectedSummaryText, result);
            // Teardown
        }

        [Fact]
        public void NameWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var product = fixture.CreateAnonymous<DiscountedProduct>();
            var sut = fixture.Build<ProductViewModel>()
                .FromFactory(() => new ProductViewModel(product))
                .OmitAutoProperties()
                .CreateAnonymous();

            var expectedName = product.Name;
            // Exercise system
            var result = sut.Name;
            // Verify outcome
            Assert.Equal<string>(expectedName, result);
            // Teardown
        }

        [Fact]
        public void UnitPriceWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var product = fixture.CreateAnonymous<DiscountedProduct>();
            var sut = fixture.Build<ProductViewModel>()
                .FromFactory(() => new ProductViewModel(product))
                .OmitAutoProperties()
                .CreateAnonymous();

            var expectedUnitPrice = product.UnitPrice;
            // Exercise system
            var result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<decimal>(expectedUnitPrice, result);
            // Teardown
        }
    }
}
