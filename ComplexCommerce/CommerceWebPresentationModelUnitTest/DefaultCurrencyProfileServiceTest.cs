using System;
using System.Web;
using System.Web.Profile;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Web.PresentationModel;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class DefaultCurrencyProfileServiceTest
    {
        [Fact]
        public void CreateWithNullHttpContextWillThrow()
        {
            // Fixture setup
            HttpContextBase nullHttpContext = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DefaultCurrencyProfileService(nullHttpContext));
            // Teardown
        }

        [Fact]
        public void SutIsCurrencyProfileService()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<DefaultCurrencyProfileService>();
            // Verify outcome
            Assert.IsAssignableFrom<CurrencyProfileService>(sut);
            // Teardown
        }

        [Fact]
        public void GetCurrencyCodeWillReturnProfileFromContextWhenThatProfileIsUserProfile()
        {
            // Fixture setup
            var fixture = new ControllerFixture();
            var expectedCurrencyCode = fixture.CreateAnonymous("CurrencyCode");

            var userProfileStub = new Mock<UserProfile>();
            userProfileStub.SetupGet(up => up.Currency).Returns(expectedCurrencyCode);

            var httpContextStub = new Mock<HttpContextBase>();
            httpContextStub.SetupGet(ctx => ctx.Profile).Returns(userProfileStub.Object);

            var sut = new DefaultCurrencyProfileService(httpContextStub.Object);
            // Exercise system
            var result = sut.GetCurrencyCode();
            // Verify outcome
            Assert.Equal<string>(expectedCurrencyCode, result);
            // Teardown
        }

        [Fact]
        public void GetCurrencyCodeWillReturnDefaultCurrencyCodeWhenProfileIsNotUserProfile()
        {
            // Fixture setup
            var profile = new Mock<ProfileBase>().Object;

            var httpContextStub = new Mock<HttpContextBase>();
            httpContextStub.SetupGet(ctx => ctx.Profile).Returns(profile);

            var sut = new DefaultCurrencyProfileService(httpContextStub.Object);
            // Exercise system
            var result = sut.GetCurrencyCode();
            // Verify outcome
            Assert.Equal<string>("DKK", result);
            // Teardown
        }

        [Fact]
        public void GetCurrencyCodeWillReturnDefaultCurrencyCodeWhenUserProfileCurrencyIsNull()
        {
            // Fixture setup
            var userProfileStub = new Mock<UserProfile>();
            userProfileStub.SetupGet(up => up.Currency).Returns((string)null);

            var httpContextStub = new Mock<HttpContextBase>();
            httpContextStub.SetupGet(ctx => ctx.Profile).Returns(userProfileStub.Object);

            var sut = new DefaultCurrencyProfileService(httpContextStub.Object);
            // Exercise system
            var result = sut.GetCurrencyCode();
            // Verify outcome
            Assert.Equal<string>("DKK", result);
            // Teardown
        }

        [Fact]
        public void GetCurrencyCodeWillReturnDefaultCurrencyCodeWhenUserProfileCurrencyIsEmpty()
        {
            // Fixture setup
            var userProfileStub = new Mock<UserProfile>();
            userProfileStub.SetupGet(up => up.Currency).Returns(string.Empty);

            var httpContextStub = new Mock<HttpContextBase>();
            httpContextStub.SetupGet(ctx => ctx.Profile).Returns(userProfileStub.Object);

            var sut = new DefaultCurrencyProfileService(httpContextStub.Object);
            // Exercise system
            var result = sut.GetCurrencyCode();
            // Verify outcome
            Assert.Equal<string>("DKK", result);
            // Teardown
        }

        [Fact]
        public void UpdateCurrencyCodeWillSaveProfile()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currencyCode = fixture.CreateAnonymous("CurrencyCode");

            var userProfileMock = new Mock<UserProfile>();
            userProfileMock.SetupSet(up => up.Currency = currencyCode).Verifiable();
            userProfileMock.Setup(up => up.Save()).Verifiable();

            var httpContextStub = new Mock<HttpContextBase>();
            httpContextStub.SetupGet(ctx => ctx.Profile).Returns(userProfileMock.Object);

            var sut = new DefaultCurrencyProfileService(httpContextStub.Object);
            // Exercise system
            sut.UpdateCurrencyCode(currencyCode);
            // Verify outcome
            userProfileMock.Verify();
            // Teardown
        }

        [Fact]
        public void UpdateCurrencyCodeWhenProfileIsNotUserProfileWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var profile = new Mock<ProfileBase>().Object;

            var httpContextStub = new Mock<HttpContextBase>();
            httpContextStub.SetupGet(ctx => ctx.Profile).Returns(profile);

            var sut = new DefaultCurrencyProfileService(httpContextStub.Object);
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                fixture.Do((string currencyCode) =>
                    sut.UpdateCurrencyCode(currencyCode)));
            // Teardown
        }
    }
}
