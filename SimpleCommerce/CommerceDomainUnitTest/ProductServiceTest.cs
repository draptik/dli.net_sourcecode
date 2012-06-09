using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using System.Security.Principal;
using Moq;
using Ploeh.SemanticComparison;
using Xunit;

namespace CommerceDomainUnitTest
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
            IEnumerable<DiscountedProduct> result = fixture.Get((IPrincipal user) => sut.GetFeaturedProducts(user));
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test deviates significantly from most of the tests in the test suite. It is more
        /// verbose, but doesn't rely on AutoFixture, so is easier to understand, which is relevant
        /// because it is used as a code sample in the book.
        /// </para>
        /// </remarks>
        [Fact]
        public void GetFeaturedProductsWillReturnCorrectProduct()
        {
            // Fixture setup
            var product = new Product();
            product.Name = "Olives";
            product.UnitPrice = 24.5m;

            var repositoryStub = new Mock<ProductRepository>();
            repositoryStub
                .Setup(s => s.GetFeaturedProducts())
                .Returns(new[] { product });

            var sut =
                new ProductService(repositoryStub.Object);

            var userStub = new Mock<IPrincipal>();
            // Exercise system
            var products = 
                sut.GetFeaturedProducts(userStub.Object);
            var result = products.Single();
            // Verify outcome
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.UnitPrice, result.UnitPrice);
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
                                    select new Likeness<Product, DiscountedProduct>(p)).ToList();

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
                                    select new Likeness<Product, DiscountedProduct>(p)
                                        .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice * .95m == d.UnitPrice)).ToList();

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
