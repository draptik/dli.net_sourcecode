using System;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;
using Ploeh.SemanticComparison;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class HomeControllerTest
    {
        [Fact]
        public void CreateWithNullProductRepositoryWillThrow()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            ProductRepository nullProductRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<HomeController>()
                    .FromFactory((CurrencyProvider cp) => new HomeController(nullProductRepository, cp))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateWithNullCurrencyProviderWillThrow()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            CurrencyProvider nullCurrencyProvider = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<HomeController>()
                    .FromFactory((ProductRepository pr) => new HomeController(pr, nullCurrencyProvider))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void DefaultCurrencyProfileServiceIsCorrect()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.Build<HomeController>()
                .OmitAutoProperties()
                .With(s => s.ControllerContext)
                .CreateAnonymous();
            Type expectedType = typeof(DefaultCurrencyProfileService);
            // Exercise system
            CurrencyProfileService result = sut.CurrencyProfileService;
            // Verify outcome
            Assert.IsAssignableFrom<DefaultCurrencyProfileService>(result);
            // Teardown
        }

        [Fact]
        public void CurrencyProfileSerivceIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.Build<HomeController>()
                .OmitAutoProperties()
                .CreateAnonymous();
            var expectedCurrencyProfileService = new Mock<CurrencyProfileService>().Object;
            // Exercise system
            sut.CurrencyProfileService = expectedCurrencyProfileService;
            CurrencyProfileService result = sut.CurrencyProfileService;
            // Verify outcome
            Assert.Equal<CurrencyProfileService>(expectedCurrencyProfileService, result);
            // Teardown
        }

        [Fact]
        public void SettingCurrencyProfileProviderToNullWillThrow()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<HomeController>();
            CurrencyProfileService nullCurrencyProfileService = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.CurrencyProfileService = nullCurrencyProfileService);
            // Teardown
        }

        [Fact]
        public void SettingCurrencyProviderServiceCanOnlyBeDoneOnce()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<HomeController>();
            var currencyProfileService1 = new Mock<CurrencyProfileService>().Object;
            var currencyProfileService2 = new Mock<CurrencyProfileService>().Object;

            sut.CurrencyProfileService = currencyProfileService1;
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                sut.CurrencyProfileService = currencyProfileService2);
            // Teardown
        }

        [Fact]
        public void SettingCurrencyProfileServiceCanNotBeDoneAfterPropertyHasBeenRead()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<HomeController>();
            var currentService = sut.CurrencyProfileService;
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                sut.CurrencyProfileService = new Mock<CurrencyProfileService>().Object);
            // Teardown
        }

        [Fact]
        public void AboutWillReturnInstance()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
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
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<HomeController>();
            Type expectedViewModelType = typeof(FeaturedProductsViewModel);
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
            var fixture = new ControllerFixture();
            var featuredProducts =
                fixture.CreateMany<Product>().ToList();
            fixture.Freeze<Mock<ProductRepository>>()
                .Setup(r => r.GetFeaturedProducts())
                .Returns(featuredProducts);

            var expectedProducts = (from p in featuredProducts
                                    select new Likeness<Product, ProductViewModel>(p)
                                        .Without(d => d.SummaryText)
                                        .Without(d => d.UnitPrice)
                                    ).ToList();

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
        public void IndexWillUseCurrencyFromProfile()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var currencyCode = fixture.CreateAnonymous("CurrencyCode");

            var currencyProfileServiceStub = new Mock<CurrencyProfileService>();
            currencyProfileServiceStub.Setup(cps => cps.GetCurrencyCode()).Returns(currencyCode);

            var currencyProviderMock = new Mock<CurrencyProvider>();
            currencyProviderMock.Setup(cp => cp.GetCurrency(currencyCode)).Returns(fixture.CreateAnonymous<Currency>()).Verifiable();
            fixture.Register(() => currencyProviderMock.Object);

            var sut = fixture.CreateAnonymous<HomeController>();
            sut.CurrencyProfileService = currencyProfileServiceStub.Object;
            // Exercise system
            sut.Index();
            // Verify outcome
            currencyProviderMock.Verify();
            // Teardown
        }

        [Fact]
        public void IndexWillReturnItemsWithCorrectCurrency()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var featuredProducts = fixture.CreateMany<Product>().ToList();
            fixture.Register(() =>
            {
                var repositoryStub = new Mock<ProductRepository>();
                repositoryStub.Setup(r => r.GetFeaturedProducts()).Returns(featuredProducts);
                return repositoryStub.Object;
            });

            var expectedCurrencyCode = fixture.CreateAnonymous("CurrencyCode");
            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(expectedCurrencyCode);
            currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(fixture.CreateAnonymous<decimal>());
            fixture.Register(() => currencyStub.Object);

            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            var result = ((FeaturedProductsViewModel)sut.Index().ViewData.Model).Products.Select(pvm => pvm.UnitPrice.CurrencyCode);
            // Verify outcome
            Assert.True(result.All(code => code == expectedCurrencyCode), "Index should convert to correct currency.");
            // Teardown
        }

        [Fact]
        public void SetCurrencyWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<HomeController>();
            // Exercise system
            RedirectToRouteResult result = fixture.Get((string currency) => sut.SetCurrency(currency));
            // Verify outcome
            Assert.Equal<string>("Index", (string)result.RouteValues["action"]);
            // Teardown
        }

        [Fact]
        public void SetCurrencyWillInteractCorrectlyWithUserProfileProvider()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var currency = fixture.CreateAnonymous("Currency");

            var currencyProfileServiceMock = new Mock<CurrencyProfileService>();
            currencyProfileServiceMock.Setup(cp => cp.UpdateCurrencyCode(currency)).Verifiable();

            var sut = fixture.CreateAnonymous<HomeController>();
            sut.CurrencyProfileService = currencyProfileServiceMock.Object;
            // Exercise system
            sut.SetCurrency(currency);
            // Verify outcome
            currencyProfileServiceMock.Verify();
            // Teardown
        }
    }
}
