using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class BasketControllerTest
    {
        [Fact]
        public void CreateWithNullBasketServiceWillThrow()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            IBasketService nullBasketService = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<BasketController>()
                    .FromFactory((CurrencyProvider cp) => new BasketController(nullBasketService, cp))
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
                fixture.Build<BasketController>()
                    .FromFactory((IBasketService bs) => new BasketController(bs, nullCurrencyProvider))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void DefaultCurrencyProfileServiceIsCorrect()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.Build<BasketController>()
                .OmitAutoProperties()
                .With(s => s.ControllerContext)
                .CreateAnonymous();
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
            var sut = fixture.Build<BasketController>()
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
            var sut = fixture.CreateAnonymous<BasketController>();
            CurrencyProfileService nullCurrencyProfileService = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.CurrencyProfileService = nullCurrencyProfileService);
            // Teardown
        }

        [Fact]
        public void SettingCurrencyProfileServiceCanOnlyBeDoneOnce()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<BasketController>();
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
            var sut = fixture.CreateAnonymous<BasketController>();
            var currentService = sut.CurrencyProfileService;
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                sut.CurrencyProfileService = new Mock<CurrencyProfileService>().Object);
            // Teardown
        }

        [Fact]
        public void IndexWillReturnInstance()
        {
            // Fixture setup
            var fixture = new ControllerFixture();

            fixture.Register(() =>
                {
                    var basketServiceStub = new Mock<IBasketService>();
                    basketServiceStub.Setup(bs => bs.GetBasketFor(It.IsAny<IPrincipal>())).Returns(fixture.CreateAnonymous<Basket>());
                    return basketServiceStub.Object;
                });

            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            ViewResult result = sut.Index();
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void IndexWillReturnCorrectViewWhenBasketIsEmpty()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            fixture.Register(() =>
                {
                    var basketServiceStub = new Mock<IBasketService>();
                    basketServiceStub.Setup(bs => bs.GetBasketFor(It.IsAny<IPrincipal>())).Returns(fixture.CreateAnonymous<Basket>());
                    return basketServiceStub.Object;
                });
            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            var result = sut.Index().ViewName;
            // Verify outcome
            Assert.Equal<string>("Empty", result);
            // Teardown
        }

        [Fact]
        public void IndexWillReturnViewWithCorrectModelWhenBasketHasContents()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            fixture.Register(() =>
            {
                var basketServiceStub = new Mock<IBasketService>();
                basketServiceStub.Setup(bs => bs.GetBasketFor(It.IsAny<IPrincipal>())).Returns(fixture.Build<Basket>().Do(b => fixture.AddManyTo(b.Contents)).CreateAnonymous());
                return basketServiceStub.Object;
            });
            var sut = fixture.CreateAnonymous<BasketController>();
            Type expectedModelType = typeof(BasketViewModel);
            // Exercise system
            var result = sut.Index().ViewData.Model;
            // Verify outcome
            Assert.IsAssignableFrom<BasketViewModel>(result);
            // Teardown
        }

        [Fact]
        public void IndexWillUseCurrencyFromCurrencyProfileService()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var currencyCode = fixture.CreateAnonymous("CurrencyCode");

            var currencyProfileServiceStub = new Mock<CurrencyProfileService>();
            currencyProfileServiceStub.Setup(cps => cps.GetCurrencyCode()).Returns(currencyCode);

            var currencyProviderMock = new Mock<CurrencyProvider>();
            currencyProviderMock.Setup(cp => cp.GetCurrency(currencyCode)).Returns(fixture.CreateAnonymous<Currency>()).Verifiable();
            fixture.Register(() => currencyProviderMock.Object);

            fixture.Register(() =>
                {
                    var basketServiceStub = new Mock<IBasketService>();
                    basketServiceStub.Setup(bs => bs.GetBasketFor(It.IsAny<IPrincipal>())).Returns(fixture.CreateAnonymous<Basket>());
                    return basketServiceStub.Object;
                });

            var sut = fixture.CreateAnonymous<BasketController>();
            sut.CurrencyProfileService = currencyProfileServiceStub.Object;
            // Exercise system
            sut.Index();
            // Verify outcome
            currencyProviderMock.Verify();
            // Teardown
        }

        [Fact]
        public void IndexWillReturnCorrectContentsWhenProfileRequestsConversion()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var basket = fixture.CreateAnonymous<Basket>();
            fixture.AddManyTo(basket.Contents);

            var currencyCode = fixture.CreateAnonymous("CurrencyCode");
            var anonymousExchangeRate = 3.2m;

            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(currencyCode);
            currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(anonymousExchangeRate);

            fixture.Register(() =>
                {
                    var basketServiceStub = new Mock<IBasketService>();
                    basketServiceStub.Setup(bs => bs.GetBasketFor(It.IsAny<IPrincipal>())).Returns(basket);
                    return basketServiceStub.Object;
                });
            fixture.Customize<UserProfile>(ob => ob.FromFactory(() =>
                {
                    var profileStub = new Mock<UserProfile>();
                    profileStub.SetupGet(p => p.Currency).Returns(currencyCode);
                    return profileStub.Object;
                }).OmitAutoProperties());
            fixture.Register(() =>
                {
                    var currencyProviderStub = new Mock<CurrencyProvider>();
                    currencyProviderStub.Setup(cp => cp.GetCurrency(currencyCode)).Returns(currencyStub.Object);
                    return currencyProviderStub.Object;
                });

            var expectedContents = new BasketViewModel(basket.ConvertTo(currencyStub.Object)).Contents.ToList();

            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            var result = ((BasketViewModel)sut.Index().ViewData.Model).Contents.ToList();
            // Verify outcome
            Assert.True(expectedContents.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void AddWillReturnCorrectRedirectAction()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            RedirectToRouteResult result = fixture.Get((int id) => sut.Add(id));
            // Verify outcome
            Assert.Equal<string>("Index", (string)result.RouteValues["action"]);
            // Teardown
        }

        [Fact]
        public void AddWillAddCorrectItemToBasketService()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            
            var productId = fixture.CreateAnonymous<int>();

            var repositoryStub = new Mock<IBasketService>();
            repositoryStub.Setup(r => r.AddToBasket(productId, 1, It.IsAny<IPrincipal>())).Verifiable();

            fixture.Register(() => repositoryStub.Object);

            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            sut.Add(productId);
            // Verify outcome
            repositoryStub.Verify();
            // Teardown
        }

        [Fact]
        public void EmptyWillReturnCorrectRedirectAction()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            RedirectToRouteResult result = sut.Empty();
            // Verify outcome
            Assert.Equal<string>("Index", (string)result.RouteValues["action"]);
            // Teardown
        }

        [Fact]
        public void EmptyWillEmptyBasketFromService()
        {
            // Fixture setup
            var fixture = new ControllerFixture();

            var user = new Mock<IPrincipal>().Object;
            fixture.Register(() => user);

            var repositoryStub = new Mock<IBasketService>();
            repositoryStub.Setup(r => r.Empty(user)).Verifiable();

            fixture.Register(() => repositoryStub.Object);

            var sut = fixture.CreateAnonymous<BasketController>();
            // Exercise system
            sut.Empty();
            // Verify outcome
            repositoryStub.Verify();
            // Teardown
        }
    }
}
