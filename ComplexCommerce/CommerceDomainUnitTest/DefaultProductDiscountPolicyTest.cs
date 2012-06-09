using System.Linq;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class DefaultProductDiscountPolicyTest
    {
        [Fact]
        public void SutIsBasketDiscountPolicy()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<DefaultProductDiscountPolicy>();
            // Verify outcome
            Assert.IsAssignableFrom<BasketDiscountPolicy>(sut);
            // Teardown
        }

        [Fact]
        public void ApplyWillApplyDefaultProductDiscountForPreferredCustomer()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();

            fixture.Register(() =>
                {
                    var customerStub = new Mock<IPrincipal>();
                    customerStub.Setup(p => p.IsInRole("PreferredCustomer")).Returns(true);
                    return customerStub.Object;
                });

            var basket = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(basket.Contents);

            var expectedExtents = (from e in basket.Contents
                                   let ep = new DefaultCustomerDiscountPolicy().Apply(e.Product, basket.Owner)
                                   select new Likeness<Extent, Extent>(e)
                                        .With(d => d.Product).EqualsWhen((s, d) => new Likeness<Product, Product>(ep)
                                            .Equals(d.Product))
                                        .Without(d => d.Total)
                                        .Without(d => d.Updated))
                                    .ToList();

            var sut = fixture.CreateAnonymous<DefaultProductDiscountPolicy>();
            // Exercise system
            var result = sut.Apply(basket);
            // Verify outcome
            Assert.True(expectedExtents.Cast<object>().SequenceEqual(result.Contents.Cast<object>()));
            // Teardown
        }
    }
}
