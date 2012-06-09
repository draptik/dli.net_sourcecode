using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    public class MoneyViewModelTest
    {
        [Theory, AutoMoqData]
        public void AmountIsProperWritableProperty(decimal expectedAmount, MoneyViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Amount = expectedAmount;
            decimal result = sut.Amount;
            // Verify outcome
            Assert.Equal(expectedAmount, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CurrencyIsProperWritableProperty(string expectedCurrency, MoneyViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.CurrencyCode = expectedCurrency;
            string result = sut.CurrencyCode;
            // Verify outcome
            Assert.Equal(expectedCurrency, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ToStringReturnsCorrectResult(MoneyViewModel sut)
        {
            // Fixture setup
            var expectedResult = string.Format("{0} {1:F}", sut.CurrencyCode, sut.Amount);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
