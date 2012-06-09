using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class ProductServiceTest
    {
        [Fact]
        public void CreateWithNullRepositoryWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            ProductRepository nullRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<ProductService>()
                    .FromFactory(() => new ProductService(nullRepository))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void GetFeaturedProductsWithNullPrincipalWillThrow()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var sut = fixture.CreateAnonymous<ProductService>();
            IPrincipal nullPrincipal = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetFeaturedProducts(nullPrincipal));
            // Teardown
        }

        [Fact]
        public void GetFeaturedProductsWillReturnInstance()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var sut = fixture.CreateAnonymous<ProductService>();
            // Exercise system
            IEnumerable<Product> result = fixture.Get((IPrincipal user) => sut.GetFeaturedProducts(user));
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void GetFeaturedProductsWillReturnCorrectProductsForNonPreferredUser()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var featuredProducts = fixture.CreateMany<Product>().ToList();
            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<ProductRepository>();
                    repositoryStub.Setup(r => r.GetFeaturedProducts()).Returns(featuredProducts);
                    return repositoryStub.Object;
                });

            var expectedProducts = (from p in featuredProducts
                                    select new Likeness<Product, Product>(p)).ToList();

            var sut = fixture.CreateAnonymous<ProductService>();
            // Exercise system
            var result = fixture.Get((IPrincipal user) => sut.GetFeaturedProducts(user));
            // Verify outcome
            Assert.True(expectedProducts.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }

        [Fact]
        public void GetFeaturedProductsWillReturnCorrectProductsForPreferredCustomer()
        {
            // Fixture setup
            var fixture = new RepositoryFixture();
            var featuredProducts = fixture.CreateMany<Product>().ToList();
            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<ProductRepository>();
                    repositoryStub.Setup(r => r.GetFeaturedProducts()).Returns(featuredProducts);
                    return repositoryStub.Object;
                });

            var expectedProducts = (from p in featuredProducts
                                    select new Likeness<Product, Product>(p)
                                        .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice.Multiply(.95m).Equals(d.UnitPrice)))
                                    .ToList();

            var sut = fixture.CreateAnonymous<ProductService>();

            var preferredCustomerStub = new Mock<IPrincipal>();
            preferredCustomerStub.Setup(u => u.IsInRole("PreferredCustomer")).Returns(true);
            // Exercise system
            var result = sut.GetFeaturedProducts(preferredCustomerStub.Object);
            // Verify outcome
            Assert.True(expectedProducts.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }
    }
}
