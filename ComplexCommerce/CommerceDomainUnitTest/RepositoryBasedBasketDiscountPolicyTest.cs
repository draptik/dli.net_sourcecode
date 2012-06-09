using System;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Ploeh.SemanticComparison.Fluent;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class RepositoryBasedBasketDiscountPolicyTest
    {
        [Fact]
        public void CreateWithNullRepositoryWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            DiscountRepository nullRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<RepositoryBasketDiscountPolicy>()
                    .FromFactory(() => new RepositoryBasketDiscountPolicy(nullRepository))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void SutIsBasketDiscountPolicy()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            Type expectedBase = typeof(BasketDiscountPolicy);
            // Exercise system
            var sut = fixture.CreateAnonymous<RepositoryBasketDiscountPolicy>();
            // Verify outcome
            Assert.IsAssignableFrom<BasketDiscountPolicy>(sut);
            // Teardown
        }

        [Fact]
        public void ApplyWillApplyDiscountFromRepository()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var discountedProducts = fixture.CreateMany<Product>().ToList();

            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<DiscountRepository>();
                    repositoryStub.Setup(r => r.GetDiscountedProducts()).Returns(discountedProducts);
                    return repositoryStub.Object;
                });

            var basket = fixture.CreateAnonymous<Basket>();
            basket.Contents.Add(new Extent(discountedProducts.First().WithUnitPrice(discountedProducts.First().UnitPrice.Add(fixture.CreateAnonymous<decimal>()))) { Quantity = fixture.CreateAnonymous<int>() });

            var expectedExtent = new
            {
                Product = new Likeness<Product, Product>(discountedProducts.First()).Without(d => d.Id),
                basket.Contents.First().Quantity
            }.AsSource().OfLikeness<Extent>()
            .Without(d => d.Total)
            .Without(d => d.Updated);


            var sut = fixture.CreateAnonymous<RepositoryBasketDiscountPolicy>();
            // Exercise system
            var result = sut.Apply(basket);
            // Verify outcome
            Assert.True(expectedExtent.Equals(result.Contents.First()));
            // Teardown
        }

        [Fact]
        public void ApplyWillNotApplyDiscountsIfNoneInBasketArePresentInRepository()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();

            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<DiscountRepository>();
                    repositoryStub.Setup(r => r.GetDiscountedProducts()).Returns(fixture.CreateMany<Product>());
                    return repositoryStub.Object;
                });

            var basket = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(basket.Contents);

            var expectedContents = (from e in basket.Contents
                                    select new Likeness<Extent, Extent>(e)
                                        .With(d => d.Product).EqualsWhen((s, d) => new Likeness<Product, Product>(s.Product).Equals(d.Product)))
                                    .ToList();

            var sut = fixture.CreateAnonymous<RepositoryBasketDiscountPolicy>();
            // Exercise system
            var result = sut.Apply(basket);
            // Verify outcome
            Assert.True(expectedContents.Cast<object>().SequenceEqual(result.Contents.Cast<object>()));
            // Teardown
        }
    }
}
