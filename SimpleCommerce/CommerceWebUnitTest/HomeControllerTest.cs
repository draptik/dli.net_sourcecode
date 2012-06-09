using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ploeh.Samples.Commerce.Web.Controllers;
using Ploeh.Samples.Commerce.Web.Models;
using Ploeh.Samples.Commerce.Domain;
using Moq;
using Ploeh.AutoFixture;
using System.Security.Principal;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.WebUnitTest
{
    public class HomeControllerTest
    {
        [Fact]
        public void CreateWithNullProductRepositoryWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            ProductRepository nullProductRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<HomeController>()
                    .FromFactory(() => new HomeController(nullProductRepository))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void AboutWillReturnInstance()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            ViewResult result = sut.About();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void IndexWillReturnViewWithCorrectModel()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            ViewResult result = sut.Index();
            // Verify outcome
            Assert.IsAssignableFrom<FeaturedProductsViewModel>(result.ViewData.Model);
            // Teardown
        }

        [Fact]
        public void IndexWillReturnViewModelWithCorrectProducts()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            var featuredProducts =
                fixture.CreateMany<Product>().ToList();
            fixture.Register(() =>
                {
                    var repositoryStub =
                        new Mock<ProductRepository>();
                    repositoryStub.Setup(r =>
                        r.GetFeaturedProducts())
                        .Returns(featuredProducts);
                    return repositoryStub.Object;
                });

            var expectedProducts = (from p in featuredProducts
                                    select new Likeness<Product, ProductViewModel>(p).Without(d => d.SummaryText)).ToList();

            var sut =
                fixture.CreateAnonymous<HomeController>();
            // Exercise system
            var result = ((FeaturedProductsViewModel)sut
                .Index().ViewData.Model).Products.ToList();
            // Verify outcome
            Assert.True(expectedProducts.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }

        [Fact]
        public void IndexWillReturnViewModelWithCorrectProductsWhenUserIsPreferred()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            var featuredProducts = fixture.CreateMany<Product>().ToList();
            fixture.Register(() =>
                {
                    var repositoryStub = new Mock<ProductRepository>();
                    repositoryStub.Setup(r => r.GetFeaturedProducts()).Returns(featuredProducts);
                    return repositoryStub.Object;
                });
            fixture.Register(() =>
                {
                    var userStub = new Mock<IPrincipal>();
                    userStub.Setup(u => u.IsInRole("PreferredCustomer")).Returns(true);
                    return userStub.Object;
                });

            var expectedProducts = (from p in featuredProducts
                                    select new Likeness<Product, ProductViewModel>(new Product() 
                                    {
                                        Name = p.Name, 
                                        UnitPrice = p.UnitPrice * .95m
                                    })
                                    .Without(d => d.SummaryText)).ToList();

            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            var result = ((FeaturedProductsViewModel)sut.Index().ViewData.Model).Products.ToList();
            // Verify outcome
            Assert.True(expectedProducts.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }
    }
}
