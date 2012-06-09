using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Moq;
using System.Security.Principal;
using Xunit;

namespace CommerceDomainUnitTest
{
    public class ProductTest
    {
        [Fact]
        public void NameIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Product>();
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
            var sut = fixture.CreateAnonymous<Product>();
            decimal expectedUnitPrice = fixture.CreateAnonymous<decimal>();
            // Exercise system
            sut.UnitPrice = expectedUnitPrice;
            decimal result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<decimal>(expectedUnitPrice, result);
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
            decimal expctedUnitPrice = sut.UnitPrice;
            // Exercise system
            var result = sut.ApplyDiscountFor(userStub.Object);
            // Verify outcome
            Assert.Equal<decimal>(expctedUnitPrice, result.UnitPrice);
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
            decimal expectedUnitPrice = sut.UnitPrice * .95m;
            // Exercise system
            var result = sut.ApplyDiscountFor(userStub.Object);
            // Verify outcome
            Assert.Equal<decimal>(expectedUnitPrice, result.UnitPrice);
            // Teardown
        }
    }
}
