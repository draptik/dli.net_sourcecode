using System;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class MoneyTest
    {
        [Fact]
        public void CreateWithNullCurrencyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            string nullCurrency = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                fixture.Build<Money>()
                    .FromFactory((decimal amount) => new Money(amount, nullCurrency))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void CreateWithEmptyCurrencyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            string emptyCurrency = string.Empty;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                fixture.Build<Money>()
                    .FromFactory((decimal amount) => new Money(amount, emptyCurrency))
                    .OmitAutoProperties()
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void AmountWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            decimal expectedAmount = fixture.CreateAnonymous<decimal>();
            var sut = fixture.Build<Money>()
                .FromFactory((string currency) => new Money(expectedAmount, currency))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            decimal result = sut.Amount;
            // Verify outcome
            Assert.Equal<decimal>(expectedAmount, result);
            // Teardown
        }

        [Fact]
        public void CurrencyWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            string expectedCurrency = fixture.CreateAnonymous("Currency");
            var sut = fixture.Build<Money>()
                .FromFactory((decimal amount) => new Money(amount, expectedCurrency))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            string result = sut.CurrencyCode;
            // Verify outcome
            Assert.Equal<string>(expectedCurrency, result);
            // Teardown
        }

        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<Money>();
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<Money>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNull()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            object nullObject = null;
            // Exercise system
            bool result = sut.Equals(nullObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullMoney()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            Money nullMoney = null;
            // Exercise system
            bool result = sut.Equals(nullMoney);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualSomeObject()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            object anonymousObject = new object();
            // Exercise system
            bool result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void EqualsWillBeFalseWhenAmountsDiffer()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            var other = fixture.Build<Money>()
                .FromFactory((decimal amount) => new Money(amount, sut.CurrencyCode))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void EquaWillBeFalseWhenCurrenciesDiffer()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            var other = fixture.Build<Money>()
                .FromFactory((string currency) => new Money(sut.Amount, currency))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void EqualsWillBeTrueWhenBothAmountAndCurrencyAreEqual()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            var other = new Money(sut.Amount, sut.CurrencyCode);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void EqualsWillBeTrueWhenBothAmountAndCurrencyAreEqualButOtherIsDeclaredAsObject()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            object other = new Money(sut.Amount, sut.CurrencyCode);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void GetHashCodeWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            int expectedHashCode = sut.Amount.GetHashCode() ^ sut.CurrencyCode.GetHashCode();
            // Exercise system
            int result = sut.GetHashCode();
            // Verify outcome
            Assert.Equal<int>(expectedHashCode, result);
            // Teardown
        }

        [Fact]
        public void ToStringWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            string expectedString = string.Format("{0:F} {1}", sut.Amount, sut.CurrencyCode);
            // Exercise system
            string result = sut.ToString();
            // Verify outcome
            Assert.Equal<string>(expectedString, result);
            // Teardown
        }

        [Fact]
        public void MultiplyWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var multiplier = fixture.CreateMany<decimal>().Sum(); // Ensures that the multiplier is greater than 1
            var sut = fixture.CreateAnonymous<Money>();
            var expectedMoney = new Money(sut.Amount * multiplier, sut.CurrencyCode);
            // Exercise system
            Money result = sut.Multiply(multiplier);
            // Verify outcome
            Assert.Equal<Money>(expectedMoney, result);
            // Teardown
        }

        [Fact]
        public void AddNumberWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var addend = fixture.CreateAnonymous<decimal>();
            var sut = fixture.CreateAnonymous<Money>();
            var expectedMoney = new Money(sut.Amount + addend, sut.CurrencyCode);
            // Exercise system
            Money result = sut.Add(addend);
            // Verify outcome
            Assert.Equal<Money>(expectedMoney, result);
            // Teardown
        }

        [Fact]
        public void ConvertToNullCurrencyWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();
            Currency nullCurrency = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.ConvertTo(nullCurrency));
            // Teardown
        }

        [Fact]
        public void ConvertWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<Money>();

            var exchangeRate = fixture.CreateAnonymous<decimal>() * fixture.CreateAnonymous<decimal>() + fixture.CreateAnonymous<decimal>() / fixture.CreateAnonymous<decimal>();
            var currencyStub = new Mock<Currency>();
            currencyStub.SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("CurrencyCode"));
            currencyStub.Setup(c => c.GetExchangeRateFor(sut.CurrencyCode)).Returns(exchangeRate);

            Money expectedResult = new Money(sut.Amount * exchangeRate, currencyStub.Object.Code);
            // Exercise system
            Money result = sut.ConvertTo(currencyStub.Object);
            // Verify outcome
            Assert.Equal<Money>(expectedResult, result);
            // Teardown
        }
    }
}
