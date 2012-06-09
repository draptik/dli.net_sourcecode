using System;
using System.Linq;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class BasketServiceTest
    {
        [Fact]
        public void CreateWithNullRepositoryWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            BasketRepository nullRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<BasketService>()
                    .FromFactory((BasketDiscountPolicy bdp) => new BasketService(nullRepository, bdp))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateWithNullBasketDiscountPolicyWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            BasketDiscountPolicy nullDiscountPolicy = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<BasketService>()
                    .FromFactory((BasketRepository r) => new BasketService(r, nullDiscountPolicy))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void SutImplementsIBasketService()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<BasketService>();
            // Verify outcome
            Assert.IsAssignableFrom<IBasketService>(sut);
            // Teardown
        }

        [Fact]
        public void GetBasketForWithNullUserWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var sut = fixture.CreateAnonymous<BasketService>();
            IPrincipal nullUser = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetBasketFor(nullUser));
            // Teardown
        }

        [Fact]
        public void GetBasketWhenRepositoryHasNoBasketWillReturnEmptyBasket()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<BasketRepository>();
                    repositoryStub.Setup(r => r.GetBasketFor(It.IsAny<IPrincipal>())).Returns(Enumerable.Empty<Extent>());
                    return repositoryStub.Object;
                });
            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            Basket result = fixture.Get((IPrincipal user) => sut.GetBasketFor(user));
            // Verify outcome
            Assert.False(result.Contents.Any(), "GetBasketFor");
            // Teardown
        }

        [Fact]
        public void GetBasketWhenRepositoryHasBasketWillReturnBasketWithCorrectContents()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var products = fixture.CreateMany<Extent>().ToList();
            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<BasketRepository>();
                    repositoryStub.Setup(r => r.GetBasketFor(It.IsAny<IPrincipal>())).Returns(products);
                    return repositoryStub.Object;
                });
            var expectedItems = (from e in products
                                 select new Likeness<Extent, Extent>(e)
                                    .With(d => d.Product).EqualsWhen((s, d) => new Likeness<Product, Product>(s.Product).Equals(d.Product))
                                    .Without(d => d.Total)
                                    .Without(d => d.Updated))
                                .ToList();
            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            var result = fixture.Get((IPrincipal user) => sut.GetBasketFor(user));
            // Verify outcome
            Assert.True(expectedItems.Cast<object>().SequenceEqual(result.Contents.Cast<object>()));
            // Teardown
        }

        [Fact]
        public void GetBasketWillApplyDiscountPolicyOnBasket()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();

            var discountPolicyMock = new Mock<BasketDiscountPolicy>();
            discountPolicyMock.Setup(p => p.Apply(It.IsAny<Basket>())).Returns((Basket b) => b);
            fixture.Register(() => discountPolicyMock.Object);

            fixture.Register(() =>
                {
                    var basketRepositoryStub = new Mock<BasketRepository>();
                    basketRepositoryStub.Setup(r => r.GetBasketFor(It.IsAny<IPrincipal>())).Returns(fixture.CreateMany<Extent>());
                    return basketRepositoryStub.Object;
                });

            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            var basket = fixture.Get((IPrincipal user) => sut.GetBasketFor(user));
            // Verify outcome
            discountPolicyMock.Verify(p => p.Apply(basket), "Discount policy not correctly invoked");
            // Teardown
        }

        [Fact]
        public void GetBasketWillReturnBasketWithAppliedDiscountPolicy()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();

            var expectedBasket = fixture.CreateAnonymous<Basket>();

            var discountPolicyMock = new Mock<BasketDiscountPolicy>();
            discountPolicyMock.Setup(dp => dp.Apply(It.IsAny<Basket>())).Returns(expectedBasket);
            fixture.Register(() => discountPolicyMock.Object);

            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            var result = fixture.Get((IPrincipal user) => sut.GetBasketFor(user));
            // Verify outcome
            Assert.Equal<Basket>(expectedBasket, result);
            // Teardown
        }

        [Fact]
        public void AddToBasketWillUseRepositoryCorrectly()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();

            var productId = fixture.CreateAnonymous<int>();
            var quantity = fixture.CreateAnonymous<int>();
            var user = fixture.CreateAnonymous<IPrincipal>();

            var repositoryMock = new Mock<BasketRepository>();
            repositoryMock.Setup(r => r.AddToBasket(productId, quantity, user)).Verifiable();
            fixture.Register(() => repositoryMock.Object);

            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            sut.AddToBasket(productId, quantity, user);
            // Verify outcome
            repositoryMock.Verify();
            // Teardown
        }

        [Fact]
        public void EmptyWillUseRepositoryCorrectly()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var user = fixture.CreateAnonymous<IPrincipal>();

            var repositoryMock = new Mock<BasketRepository>();
            repositoryMock.Setup(r => r.Empty(user)).Verifiable();
            fixture.Register(() => repositoryMock.Object);

            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            sut.Empty(user);
            // Verify outcome
            repositoryMock.Verify();
            // Teardown
        }

        [Fact]
        public void GetAllBasketsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var baskets = fixture.CreateMany<Basket>().ToList();

            fixture.Freeze<Mock<BasketRepository>>().Setup(r => r.GetAllBaskets()).Returns(baskets);

            var discountPolicyStub = fixture.Freeze<Mock<BasketDiscountPolicy>>();
            var discountedBaskets = baskets.Select(b =>
                {
                    var discountedBasket = fixture.CreateAnonymous<Basket>();
                    discountPolicyStub.Setup(dp => dp.Apply(b)).Returns(discountedBasket);
                    return discountedBasket;
                }).ToList();

            var sut = fixture.CreateAnonymous<BasketService>();
            // Exercise system
            var result = sut.GetAllBaskets();
            // Verify outcome
            Assert.True(discountedBaskets.SequenceEqual(result));
            // Teardown
        }
    }
}
