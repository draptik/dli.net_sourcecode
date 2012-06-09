using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class DefaultCustomerDiscountPolicyTest
    {
        [Fact]
        public void ApplyDiscountForNormalCustomerWillReturnEquivalentProduct()
        {
            // Fixture setup
            var fixture = new Fixture();

            var customerStub = new Mock<IPrincipal>();
            customerStub.Setup(c => c.IsInRole("PreferredCustomer")).Returns(false);

            var product = fixture.CreateAnonymous<Product>();

            var expectedResult = new Likeness<Product, Product>(product);

            var sut = fixture.CreateAnonymous<DefaultCustomerDiscountPolicy>();
            // Exercise system
            Product result = sut.Apply(product, customerStub.Object);
            // Verify outcome
            Assert.True(expectedResult.Equals(result));
            // Teardown
        }

        [Fact]
        public void ApplyDiscountForPreferredCustomerWillReturnCorrectlyDiscountedProduct()
        {
            // Fixture setup
            var fixture = new Fixture();

            var customerStub = new Mock<IPrincipal>();
            customerStub.Setup(c => c.IsInRole("PreferredCustomer")).Returns(true);

            var product = fixture.CreateAnonymous<Product>();

            var expectedResult = new Likeness<Product, Product>(product)
                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice.Multiply(.95m).Equals(d.UnitPrice));

            var sut = fixture.CreateAnonymous<DefaultCustomerDiscountPolicy>();
            // Exercise system
            var result = sut.Apply(product, customerStub.Object);
            // Verify outcome
            Assert.True(expectedResult.Equals(result));
            // Teardown
        }
    }
}
