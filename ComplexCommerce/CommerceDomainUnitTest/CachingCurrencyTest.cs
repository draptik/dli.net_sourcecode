using System;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Ploeh.AutoFixture.AutoMoq;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class CachingCurrencyTest : IDisposable
    {
        [Fact]
        public void CreateWithNullInnerCurrencyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            Currency nullCurrency = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CachingCurrency(nullCurrency, fixture.CreateAnonymous<TimeSpan>()));
            // Teardown (implicit)
        }

        [Fact]
        public void SutIsCurrency()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Register(() => new Mock<Currency>().Object);
            // Exercise system
            var sut = fixture.CreateAnonymous<CachingCurrency>();
            // Verify outcome
            Assert.IsAssignableFrom<Currency>(sut);
            // Teardown (implicit)
        }

        [Fact]
        public void CacheTimeoutWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedTimeout = fixture.CreateAnonymous<TimeSpan>();
            var sut = new CachingCurrency(new Mock<Currency>().Object, expectedTimeout);
            // Exercise system
            TimeSpan result = sut.CacheTimeout;
            // Verify outcome
            Assert.Equal<TimeSpan>(expectedTimeout, result);
            // Teardown (implicit)
        }

        [Fact]
        public void CodeWillMatchInnerCurrency()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedCurrencyCode = fixture.CreateAnonymous("CurrencyCode");

            var innerCurrencyStub = new Mock<Currency>();
            innerCurrencyStub.SetupGet(c => c.Code).Returns(expectedCurrencyCode);
            fixture.Register(() => innerCurrencyStub.Object);

            var sut = fixture.CreateAnonymous<CachingCurrency>();
            // Exercise system
            var result = sut.Code;
            // Verify outcome
            Assert.Equal<string>(expectedCurrencyCode, result);
            // Teardown (implicit)
        }

        [Theory, AutoMoqData]
        public void GetExchangeRateOnceWillReturnRateFromInnerCurrency(decimal expectedExchangeRate, string currencyCode, [Frozen]Mock<Currency> innerCurrencyStub, CachingCurrency sut)
        {
            // Fixture setup
            innerCurrencyStub.Setup(c => c.GetExchangeRateFor(currencyCode)).Returns(expectedExchangeRate);
            // Exercise system
            var result = sut.GetExchangeRateFor(currencyCode);
            // Verify outcome
            Assert.Equal<decimal>(expectedExchangeRate, result);
            // Teardown (implicit)
        }

        [Fact]
        public void InnerCurrencyIsOnlyInvokedOnceDespiteMultipleCalls()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var currencyCode = fixture.CreateAnonymous("CurrencyCode");

            var innerCurrencyMock = fixture.Freeze<Mock<Currency>>();
            innerCurrencyMock.Setup(c => c.GetExchangeRateFor(currencyCode)).Returns(fixture.CreateAnonymous<decimal>()).Verifiable();

            fixture.Register(() => TimeSpan.FromMinutes(1));

            var sut = fixture.CreateAnonymous<CachingCurrency>();
            // Exercise system
            sut.GetExchangeRateFor(currencyCode);
            sut.GetExchangeRateFor(currencyCode);
            // Verify outcome
            innerCurrencyMock.Verify(c => c.GetExchangeRateFor(currencyCode), Times.Once());
            // Teardown (implicit)
        }

        [Fact]
        public void InnerCurrencyIsInvokedAgainWhenCacheExpires()
        {
            // Fixture setup
            var currencyCode = "CHF";
            var cacheTimeout = TimeSpan.FromHours(1);

            var startTime = new DateTime(2009, 8, 29);

            var timeProviderStub = new Mock<TimeProvider>();
            timeProviderStub
                .SetupGet(tp => tp.UtcNow)
                .Returns(startTime);
            TimeProvider.Current = timeProviderStub.Object;

            var innerCurrencyMock = new Mock<Currency>();
            innerCurrencyMock
                .Setup(c => c.GetExchangeRateFor(currencyCode))
                .Returns(4.911m)
                .Verifiable();

            var sut =
                new CachingCurrency(innerCurrencyMock.Object,
                    cacheTimeout);
            sut.GetExchangeRateFor(currencyCode);
            sut.GetExchangeRateFor(currencyCode);
            sut.GetExchangeRateFor(currencyCode);

            timeProviderStub
                .SetupGet(tp => tp.UtcNow)
                .Returns(startTime + cacheTimeout);
            // Exercise system
            sut.GetExchangeRateFor(currencyCode);
            // Verify outcome
            innerCurrencyMock.Verify(
                c => c.GetExchangeRateFor(currencyCode),
                Times.Exactly(2));
            // Teardown (implicit)
        }

        #region IDisposable Members

        public void Dispose()
        {
            TimeProvider.ResetToDefault();
        }

        #endregion
    }
}
