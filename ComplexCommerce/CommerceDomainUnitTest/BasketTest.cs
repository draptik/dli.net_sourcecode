using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class BasketTest
    {
        [Fact]
        public void CreateWithNullOwnerWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            IPrincipal nullOwner = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<Basket>()
                    .FromFactory(() => new Basket(nullOwner))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void OwnerWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var expectedOwner = fixture.CreateAnonymous<IPrincipal>();
            var sut = fixture.Build<Basket>()
                .FromFactory(() => new Basket(expectedOwner))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            IPrincipal result = sut.Owner;
            // Verify outcome
            Assert.Equal<IPrincipal>(expectedOwner, result);
            // Teardown
        }

        [Fact]
        public void ContentsIsNotNull()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var sut = fixture.CreateAnonymous<Basket>();
            // Exercise system
            IList<Extent> result = sut.Contents;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ConvertToNullWillThrow()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            fixture.Register(() => new Mock<IPrincipal>().Object);

            var sut = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(sut.Contents);

            Currency nullCurrency = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.ConvertTo(nullCurrency));
            // Teardown
        }

        [Fact]
        public void ConvertToWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            fixture.Register(() => new Mock<IPrincipal>().Object);

            var sut = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(sut.Contents);

            var currency = fixture.CreateAnonymous<Currency>();
            var expectedContents = (from x in sut.Contents
                                    let convertedProduct = x.Product.ConvertTo(currency)
                                    select new Likeness<Extent, Extent>(x)
                                        .With(d => d.Product).EqualsWhen((s, d) => new Likeness<Product, Product>(convertedProduct).Equals(d.Product))
                                        .Without(d => d.Total)
                                        .Without(d => d.Updated))
                                    .ToList();
            // Exercise system
            Basket result = sut.ConvertTo(currency);
            // Verify outcome
            Assert.True(expectedContents.Cast<object>().SequenceEqual(result.Contents.Cast<object>()));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UpdatedForEmptyBasketIsCorrect(Basket sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.Updated;
            // Verify outcome
            Assert.Equal(DateTimeOffset.MinValue, result);
            // Teardown
        }

        [Fact]
        public void UpdatedIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var sut = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(sut.Contents);
            // Exercise system
            DateTimeOffset result = sut.Updated;
            // Verify outcome
            var expectedUpdate = (from e in sut.Contents
                                  select e.Updated).Max();
            Assert.Equal(expectedUpdate, result);
            // Teardown
        }
    }
}
