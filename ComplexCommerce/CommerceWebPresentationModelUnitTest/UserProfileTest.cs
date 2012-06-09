using System.Web.Profile;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Web.PresentationModel;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class UserProfileTest
    {
        [Fact]
        public void SutIsProfile()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var sut = fixture.Build<UserProfile>().OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.IsAssignableFrom<ProfileBase>(sut);
            // Teardown
        }

        [Fact]
        public void SettingCurrencyWillInvokeCorrectBaseMethod()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currency = fixture.CreateAnonymous("Currency");
            var profileMock = new Mock<UserProfile>();
            profileMock.SetupSet(p => p.Currency = currency).Verifiable();
            // Exercise system
            profileMock.Object.Currency = currency;
            // Verify outcome
            profileMock.Verify();
            // Teardown
        }

        [Fact]
        public void GettingCurrencyWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            string expectedCurrency = fixture.CreateAnonymous("Currency");

            var profileMock = new Mock<UserProfile>();
            profileMock.SetupGet(p => p.Currency).Returns(expectedCurrency);
            // Exercise system
            string result = profileMock.Object.Currency;
            // Verify outcome
            Assert.Equal<string>(expectedCurrency, result);
            // Teardown
        }
    }
}
