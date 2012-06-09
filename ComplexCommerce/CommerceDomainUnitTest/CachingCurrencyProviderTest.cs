using System;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class CachingCurrencyProviderTest
    {
        [Fact]
        public void SutIsCurrencyProvider()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Register(() => new Mock<CurrencyProvider>().Object);
            // Exercise system
            var sut = fixture.CreateAnonymous<CachingCurrencyProvider>();
            // Verify outcome
            Assert.IsAssignableFrom<CurrencyProvider>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullCurrencyProviderWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            CurrencyProvider nullCurrencyProvider = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CachingCurrencyProvider(nullCurrencyProvider, fixture.CreateAnonymous<TimeSpan>()));
            // Teardown
        }

        [Fact]
        public void CacheTimeoutWillReflectConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedCacheTimeout = fixture.CreateAnonymous<TimeSpan>();
            var sut = new CachingCurrencyProvider(new Mock<CurrencyProvider>().Object, expectedCacheTimeout);
            // Exercise system
            TimeSpan result = sut.CacheTimeout;
            // Verify outcome
            Assert.Equal<TimeSpan>(expectedCacheTimeout, result);
            // Teardown
        }

        [Fact]
        public void FirstTimeAnExchangeRateIsRequestedTheResultFromTheInnerProviderIsReturned()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedExchangeRate = fixture.CreateAnonymous<decimal>();
            var sourceCurrencyCode = fixture.CreateAnonymous("CurrencyCode");
            var destinationCurrencyCode = fixture.CreateAnonymous("CurrencyCode");

            var currencyStub = new Mock<Currency>();
            currencyStub.Setup(c => c.GetExchangeRateFor(destinationCurrencyCode)).Returns(expectedExchangeRate);

            var currencyProviderStub = new Mock<CurrencyProvider>();
            currencyProviderStub.Setup(cp => cp.GetCurrency(sourceCurrencyCode)).Returns(currencyStub.Object);
            fixture.Register(() => currencyProviderStub.Object);

            var sut = fixture.CreateAnonymous<CachingCurrencyProvider>();
            // Exercise system
            var result = sut.GetCurrency(sourceCurrencyCode).GetExchangeRateFor(destinationCurrencyCode);
            // Verify outcome
            Assert.Equal<decimal>(expectedExchangeRate, result);
            // Teardown
        }

        [Fact]
        public void InnerCurrencyIsOnlyInvokedOnceDespiteMultipleCalls()
        {
            // Fixture setup
            var fixture = new Fixture();
            var exchangeRate = fixture.CreateAnonymous<decimal>();
            var sourceCurrencyCode = fixture.CreateAnonymous("CurrencyCode");
            var destinationCurrencyCode = fixture.CreateAnonymous("CurrencyCode");

            var currencyMock = new Mock<Currency>();
            currencyMock.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(exchangeRate).Verifiable();

            var currencyProviderStub = new Mock<CurrencyProvider>();
            currencyProviderStub.Setup(cp => cp.GetCurrency(sourceCurrencyCode)).Returns(currencyMock.Object);
            fixture.Register(() => currencyProviderStub.Object);

            fixture.Register(() => TimeSpan.FromMinutes(1));

            var sut = fixture.CreateAnonymous<CachingCurrencyProvider>();
            // Exercise system
            sut.GetCurrency(sourceCurrencyCode).GetExchangeRateFor(destinationCurrencyCode);
            sut.GetCurrency(sourceCurrencyCode).GetExchangeRateFor(destinationCurrencyCode);
            // Verify outcome
            currencyMock.Verify(c => c.GetExchangeRateFor(destinationCurrencyCode), Times.Once());
            // Teardown
        }

        [Fact]
        public void GetCurrencyWillReturnCachingCurrency()
        {
            // Fixture setup
            var fixture = new Fixture();

            var currencyProviderStub = new Mock<CurrencyProvider>();
            currencyProviderStub.Setup(cp => cp.GetCurrency(It.IsAny<string>())).Returns(new Mock<Currency>().Object);
            fixture.Register(() => currencyProviderStub.Object);

            var sut = fixture.CreateAnonymous<CachingCurrencyProvider>();
            // Exercise system
            var result = fixture.Get((string currencyCode) => sut.GetCurrency(currencyCode));
            // Verify outcome
            Assert.IsAssignableFrom<CachingCurrency>(result);
            // Teardown
        }

        [Fact]
        public void GetCurrencyWillReturnSameCachingInstanceForMultipleCalls()
        {
            // Fixture setup
            var fixture = new Fixture();
            var currencyCode = fixture.CreateAnonymous("CurrencyCode");

            var currencyProviderStub = new Mock<CurrencyProvider>();
            currencyProviderStub.Setup(cp => cp.GetCurrency(currencyCode)).Returns(new Mock<Currency>().Object);
            fixture.Register(() => currencyProviderStub.Object);

            var sut = fixture.CreateAnonymous<CachingCurrencyProvider>();
            var expectedCurrency = sut.GetCurrency(currencyCode);
            // Exercise system
            var result = sut.GetCurrency(currencyCode);
            // Verify outcome
            Assert.Equal<Currency>(expectedCurrency, result);
            // Teardown
        }

        [Fact]
        public void GetCurrencyWillReturnCachingCurrencyWithCorrectCacheTimeout()
        {
            // Fixture setup
            var fixture = new Fixture();

            var currencyProviderStub = new Mock<CurrencyProvider>();
            currencyProviderStub.Setup(cp => cp.GetCurrency(It.IsAny<string>())).Returns(new Mock<Currency>().Object);
            fixture.Register(() => currencyProviderStub.Object);

            var sut = fixture.CreateAnonymous<CachingCurrencyProvider>();
            var expectedTimeout = sut.CacheTimeout;
            // Exercise system
            var result = fixture.Get((string currencyCode) => (CachingCurrency)sut.GetCurrency(currencyCode));
            // Verify outcome
            Assert.Equal<TimeSpan>(expectedTimeout, result.CacheTimeout);
            // Teardown
        }
    }
}
