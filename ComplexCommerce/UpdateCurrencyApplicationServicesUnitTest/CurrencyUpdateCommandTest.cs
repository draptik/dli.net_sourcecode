using System;
using System.IO;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Moq;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.UpdateCurrency.ApplicationServices.UnitTest
{
    public class CurrencyUpdateCommandTest : IDisposable
    {
        [Theory, AutoMoqData]
        public void SutIsCommand(CurrencyUpdateCommand sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ICommand>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CurrencyIsCorrect([Frozen]Currency expectedCurrency, CurrencyUpdateCommand sut)
        {
            // Fixture setup
            // Exercise system
            Currency result = sut.Currency;
            // Verify outcome
            Assert.Equal(expectedCurrency, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DestinationCodeIsCorrect([Frozen]string expectedCode, CurrencyUpdateCommand sut)
        {
            // Fixture setup
            // Exercise system
            string result = sut.DestinationCode;
            // Verify outcome
            Assert.Equal(expectedCode, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RateIsCorrect([Frozen]decimal expectedRate, CurrencyUpdateCommand sut)
        {
            // Fixture setup
            // Exercise system
            decimal result = sut.Rate;
            // Verify outcome
            Assert.Equal(expectedRate, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ExecuteWillUpdateCurrency([Frozen]Mock<Currency> currencyMock, CurrencyUpdateCommand sut)
        {
            // Fixture setup
            // Exercise system
            sut.Execute();
            // Verify outcome
            currencyMock.Verify(c => c.UpdateExchangeRate(sut.DestinationCode, sut.Rate));
            // Teardown
        }

        [Fact]
        public void ExecuteWillWriteCorrectOutput()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Freeze<Mock<Currency>>().SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("Code"));

            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            var expectedOutput = string.Format("Updated: 1 {0} in {1} = {2}.{3}", sut.DestinationCode, sut.Currency.Code, sut.Rate, Environment.NewLine);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                // Exercise system
                sut.Execute();
                // Verify outcome
                Assert.Equal(expectedOutput, sw.ToString());
                // Teardown
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            var standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        #endregion
    }
}
