using System;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class ProductTest
    {
        [Fact]
        public void CreateWithNullNameWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            string nullName = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<Product>()
                    .FromFactory((int id, Money unitPrice) => new Product(id, nullName, unitPrice))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void IdWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            int expectedId = fixture.CreateAnonymous<int>();
            var sut = fixture.Build<Product>()
                .FromFactory((string name, Money unitPrice) => new Product(expectedId, name, unitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            int result = sut.Id;
            // Verify outcome
            Assert.Equal<int>(expectedId, result);
            // Teardown
        }

        [Fact]
        public void NameWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            string expectedName = fixture.CreateAnonymous("Name");
            var sut = fixture.Build<Product>()
                .FromFactory((int id, Money unitPrice) => new Product(id, expectedName, unitPrice))
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
            var expectedUnitPrice = fixture.CreateAnonymous<Money>();
            var sut = fixture.Build<Product>()
                .FromFactory((int id, string name) => new Product(id, name, expectedUnitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            Money result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<Money>(expectedUnitPrice, result);
            // Teardown
        }

        [Fact]
        public void ApplyDiscountWillNotApplyDiscountWhenUserIsNotPreferred()
        {
            // Fixture setup
            var fixture = new Fixture();

            var userStub = new Mock<IPrincipal>();
            userStub.Setup(u => u.IsInRole("PreferredCustomer")).Returns(false);

            var sut = fixture.CreateAnonymous<Product>();
            Money expctedUnitPrice = sut.UnitPrice;
            // Exercise system
            var result = sut.ApplyDiscountFor(userStub.Object);
            // Verify outcome
            Assert.Equal<Money>(expctedUnitPrice, result.UnitPrice);
            // Teardown
        }

        [Fact]
        public void ApplyDiscountWillApplyDiscountWhenUserIsPreferred()
        {
            // Fixture setup
            var fixture = new Fixture();

            var userStub = new Mock<IPrincipal>();
            userStub.Setup(u => u.IsInRole("PreferredCustomer")).Returns(true);

            var sut = fixture.CreateAnonymous<Product>();
            Money expectedUnitPrice = sut.UnitPrice.Multiply(.95m);
            // Exercise system
            var result = sut.ApplyDiscountFor(userStub.Object);
            // Verify outcome
            Assert.Equal<Money>(expectedUnitPrice, result.UnitPrice);
            // Teardown
        }

        [Fact]
        public void WithUnitPriceWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Product>();
            var newUnitPrice = fixture.CreateAnonymous<Money>();
            var expectedResult = new Likeness<Product, Product>(new Product(sut.Id, sut.Name, newUnitPrice));
            // Exercise system
            Product result = sut.WithUnitPrice(newUnitPrice);
            // Verify outcome
            Assert.True(expectedResult.Equals(result));
            // Teardown
        }

        [Fact]
        public void ConvertToNullCurrencyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Product>();
            Currency nullCurrency = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.ConvertTo(nullCurrency));
            // Teardown
        }

        [Fact]
        public void ConvertWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var currency = fixture.CreateAnonymous<Currency>();
            var sut = fixture.CreateAnonymous<Product>();
            var expectedProduct = new Likeness<Product, Product>(sut.WithUnitPrice(sut.UnitPrice.ConvertTo(currency)));
            // Exercise system
            Product result = sut.ConvertTo(currency);
            // Verify outcome
            Assert.True(expectedProduct.Equals(result));
            // Teardown
        }
    }
}
