using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    internal class ControllerFixture : Fixture
    {
        public ControllerFixture()
        {
            this.Customize(new AutoMoqCustomization());

            this.Register(() =>
                {
                    var currencyProviderStub = new Mock<CurrencyProvider>();
                    currencyProviderStub.Setup(cp => cp.GetCurrency(It.IsAny<string>())).Returns(this.CreateAnonymous<Currency>());
                    return currencyProviderStub.Object;
                });
            this.Register(() =>
                {
                    var currencyStub = new Mock<Currency>();
                    currencyStub.Setup(c => c.Code).Returns("DKK");
                    currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(1);
                    return currencyStub.Object;
                });

            this.Customize<BasketController>(ob => ob
                .OmitAutoProperties()
                .With(c => c.ControllerContext));
            this.Customize<HomeController>(ob => ob
                .OmitAutoProperties()
                .With(c => c.ControllerContext));

            this.Customize<UserProfile>(ob => ob.FromFactory(() =>
                {
                    var profileStub = new Mock<UserProfile>();
                    profileStub.SetupGet(p => p["Currency"]).Returns("DKK");
                    profileStub.SetupSet(p => p["Currency"] = It.IsAny<string>());
                    return profileStub.Object;
                }).OmitAutoProperties());

            this.Customize<HttpContextBase>(ob => ob
                .FromFactory(() =>
                {
                    var contextStub = new Mock<HttpContextBase>();
                    contextStub.SetupProperty(ctx => ctx.User);
                    contextStub.Object.User = this.CreateAnonymous<IPrincipal>();
                    contextStub.SetupGet(ctx => ctx.Profile).Returns(this.CreateAnonymous<UserProfile>());
                    return contextStub.Object;
                })
                .OmitAutoProperties());
        }
    }
}
