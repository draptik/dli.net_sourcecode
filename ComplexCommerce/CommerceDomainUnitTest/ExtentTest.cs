using System;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class ExtentTest
    {
        [Fact]
        public void CreateWithNullItemWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            Product nullItem = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<Extent>()
                    .FromFactory(() => new Extent(nullItem))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void ItemWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedProduct = fixture.CreateAnonymous<Product>();
            var sut = fixture.Build<Extent>()
                .FromFactory(() => new Extent(expectedProduct))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            Product result = sut.Product;
            // Verify outcome
            Assert.Equal<Product>(expectedProduct, result);
            // Teardown
        }

        [Fact]
        public void QuantityIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Extent>();
            var expectedQuantity = fixture.CreateAnonymous<int>();
            // Exercise system
            sut.Quantity = expectedQuantity;
            int result = sut.Quantity;
            // Verify outcome
            Assert.Equal<int>(expectedQuantity, result);
            // Teardown
        }

        [Fact]
        public void UpdatedIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Extent>();
            var expectedTime = fixture.CreateAnonymous<DateTimeOffset>();
            // Exercise system
            sut.Updated = expectedTime;
            DateTimeOffset result = sut.Updated;
            // Verify outcome
            Assert.Equal<DateTimeOffset>(expectedTime, result);
            // Teardown
        }

        [Fact]
        public void DefaultTotalIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<Extent>()
                .OmitAutoProperties()
                .CreateAnonymous();
            Money expectedTotal = sut.Product.UnitPrice.Multiply(sut.Quantity);
            // Exercise system
            Money result = sut.Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }

        [Fact]
        public void SettingTotalWillOverrideDefault()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Extent>();
            var expectedTotal = fixture.CreateAnonymous<Money>();
            sut.Total = expectedTotal;
            // Exercise system
            var result = sut.Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }

        [Fact]
        public void ResetTotalWillResetToDefault()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Extent>();
            Money expectedTotal = sut.Product.UnitPrice.Multiply(sut.Quantity);
            // Exercise system
            sut.ResetTotal();
            var result = sut.Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }

        [Fact]
        public void WithItemWillReturnCorrectExtent()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Extent>();
            var newItem = fixture.CreateAnonymous<Product>();
            var expectedResult = new Likeness<Extent, Extent>(new Extent(newItem) { Quantity = sut.Quantity, Updated = sut.Updated }).Without(d => d.Total);
            // Exercise system
            Extent result = sut.WithItem(newItem);
            // Verify outcome
            Assert.True(expectedResult.Equals(result));
            // Teardown
        }

        [Fact]
        public void WithItemWillReturnExtentWithCorrectTotalWhenTotalIsNotAssigned()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<Extent>()
                .Without(x => x.Total)
                .CreateAnonymous();
            var newItem = fixture.CreateAnonymous<Product>();
            var expectedTotal = newItem.UnitPrice.Multiply(sut.Quantity);
            // Exercise system
            var result = sut.WithItem(newItem).Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }

        [Fact]
        public void WithItemWillReturnExtentWithCorrectTotalWhenTotalIsAssigned()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<Extent>()
                .With(x => x.Total)
                .CreateAnonymous();
            var expectedTotal = sut.Total;
            // Exercise system
            var result = fixture.Get((Product newItem) => sut.WithItem(newItem)).Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }

        [Fact]
        public void ConvertToNullCurrencyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Extent>();
            Currency nullCurrency = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.ConvertTo(nullCurrency));
            // Teardown
        }

        [Fact]
        public void ConvertToWillPreserveQuantity()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("CurrencyCode"));
            currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(2.7m);

            var sut = fixture.CreateAnonymous<Extent>();
            var expectedQuantity = sut.Quantity;
            // Exercise system
            var result = sut.ConvertTo(currencyStub.Object).Quantity;
            // Verify outcome
            Assert.Equal<int>(expectedQuantity, result);
            // Teardown
        }

        [Fact]
        public void ConvertToWillReturnConvertedItem()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("CurrencyCode"));
            currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(2.7m);

            var item = fixture.CreateAnonymous<Product>();
            var sut = fixture.Build<Extent>()
                .FromFactory(() => new Extent(item))
                .OmitAutoProperties()
                .CreateAnonymous();
            var expectedItem = new Likeness<Product, Product>(item.ConvertTo(currencyStub.Object));
            // Exercise system
            var result = sut.ConvertTo(currencyStub.Object).Product;
            // Verify outcome
            Assert.True(expectedItem.Equals(result));
            // Teardown
        }

        [Fact]
        public void ConvertToWillConvertTotalIfAssigned()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("CurrencyCode"));
            currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(8.7m);

            var sut = fixture.Build<Extent>()
                .With(x => x.Total)
                .CreateAnonymous();
            var expectedTotal = sut.Total.ConvertTo(currencyStub.Object);
            // Exercise system
            var result = sut.ConvertTo(currencyStub.Object).Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }

        [Fact]
        public void ConvertToWillHaveCorrectTotalIfNotAssigned()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("CurrencyCode"));
            currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(8.7m);

            var sut = fixture.Build<Extent>()
                .OmitAutoProperties()
                .CreateAnonymous();
            var expectedTotal = sut.Total.ConvertTo(currencyStub.Object);
            // Exercise system
            var result = sut.ConvertTo(currencyStub.Object).Total;
            // Verify outcome
            Assert.Equal<Money>(expectedTotal, result);
            // Teardown
        }
    }
}
