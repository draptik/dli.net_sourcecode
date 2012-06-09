using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;
using Ploeh.Samples.Lifetime.MvcApplication.Models;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Lifetime.MvcApplication.UnitTest.Controllers
{
    public class HomeControllerFacts
    {
        [Theory, AutoMvcData]
        public void IndexReturnsViewResultWithDefaultViewName(HomeController controller)
        {
            // Arrange

            // Act
            ViewResult result = controller.Index();

            // Assert
            Assert.Empty(result.ViewName);
        }

        [Theory, AutoMvcData]
        public void IndexWillReturnCorrectModel(HomeController sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.Index();
            // Verify outcome
            Assert.IsAssignableFrom<HomeIndexViewModel>(result.ViewData.Model);
            // Teardown
        }

        [Fact]
        public void IndexWillAddProductToDiscountCampaign()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MvcApplicationCustomization());

            var campaignMock = fixture.CreateAnonymous<Mock<DiscountCampaign>>();
            fixture.Inject(campaignMock.Object);

            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            sut.Index();
            // Verify outcome
            campaignMock.Verify(dc => dc.AddProduct(It.Is<Product>(p => p.Name == "Success")));
            // Teardown
        }

        [Fact]
        public void IndexWillReturnCorrectViewModel()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MvcApplicationCustomization());
            var expectedProducts = fixture.CreateMany<Product>().ToList();

            fixture.Freeze<Mock<BasketDiscountPolicy>>().Setup(bdp => bdp.GetDiscountedProducts()).Returns(expectedProducts);

            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            var result = sut.Index();
            // Verify outcome
            var vm = Assert.IsAssignableFrom<HomeIndexViewModel>(result.ViewData.Model);
            Assert.True(expectedProducts.SequenceEqual(vm.Products));
            // Teardown
        }

        [Fact]
        public void IndexWillInvokeServicesInCorrectSequence()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MvcApplicationCustomization());

            var calls = new List<int>();

            var campaignSpy = fixture.CreateAnonymous<Mock<DiscountCampaign>>();
            campaignSpy
                .Setup(dc => dc.AddProduct(It.IsAny<Product>()))
                .Callback(() => calls.Add(1));
            fixture.Inject(campaignSpy.Object);

            fixture.Freeze<Mock<BasketDiscountPolicy>>()
                .Setup(bdp => bdp.GetDiscountedProducts())
                .Callback(() => calls.Add(2))
                .Returns(fixture.CreateMany<Product>());

            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            sut.Index();
            // Verify outcome
            Assert.True(Enumerable.Range(1, 2).SequenceEqual(calls));
            // Teardown
        }

        [Theory, AutoMvcData]
        public void AboutReturnsViewResultWithDefaultViewName(HomeController controller)
        {
            // Arrange

            // Act
            var result = controller.About();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Empty(viewResult.ViewName);
        }

        [Fact]
        public void AboutSetsViewDataWithNoModel()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var controller = fixture.Build<HomeController>()
                .Without(x => x.ViewData)
                .CreateAnonymous();

            // Act
            var result = controller.About();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Theory, AutoMvcData]
        public void CampaignIsCorrect([Frozen]DiscountCampaign expectedCampaign, HomeController sut)
        {
            // Fixture setup
            // Exercise system
            DiscountCampaign result = sut.Campaign;
            // Verify outcome
            Assert.Equal(expectedCampaign, result);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void PolicyIsCorrect([Frozen]BasketDiscountPolicy expectedPolicy, HomeController sut)
        {
            // Fixture setup
            // Exercise system
            BasketDiscountPolicy result = sut.Policy;
            // Verify outcome
            Assert.Equal(expectedPolicy, result);
            // Teardown
        }
    }
}