using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.Samples.Commerce.Domain;
using System.IO;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine.UnitTest
{
    [TestClass]
    public class CurrencyUpdateCommandTest
    {
        public CurrencyUpdateCommandTest()
        {
        }

        [TestCleanup]
        public void TeardownFixture()
        {
            var standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        [TestMethod]
        public void SutIsCommand()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ICommand));
            // Teardown
        }

        [TestMethod]
        public void CurrencyIsCorrect()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var expectedCurrencty = fixture.Freeze<Currency>();
            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            // Exercise system
            Currency result = sut.Currency;
            // Verify outcome
            Assert.AreEqual(expectedCurrencty, result, "Currency");
            // Teardown
        }

        [TestMethod]
        public void DestinationCodeIsCorrect()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var expectedCode = fixture.Freeze("Code");
            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            // Exercise system
            string result = sut.DestinationCode;
            // Verify outcome
            Assert.AreEqual(expectedCode, result, "DestinationCode");
            // Teardown
        }

        [TestMethod]
        public void RateIsCorrect()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var expectedRate = fixture.Freeze<decimal>();
            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            // Exercise system
            decimal result = sut.Rate;
            // Verify outcome
            Assert.AreEqual(expectedRate, result, "Rate");
            // Teardown
        }

        [TestMethod]
        public void ExecuteWillUpdateCurrency()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();

            var currencyMock = fixture.FreezeMoq<Currency>();

            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            // Exercise system
            sut.Execute();
            // Verify outcome
            currencyMock.Verify(c => c.UpdateExchangeRate(sut.DestinationCode, sut.Rate));
            // Teardown
        }

        [TestMethod]
        public void ExecuteWillWriteCorrectOutput()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();

            fixture.FreezeMoq<Currency>().SetupGet(c => c.Code).Returns(fixture.CreateAnonymous("Code"));

            var sut = fixture.CreateAnonymous<CurrencyUpdateCommand>();
            var expectedOutput = string.Format("Updated: 1 {0} in {1} = {2}.{3}", sut.DestinationCode, sut.Currency.Code, sut.Rate, Environment.NewLine);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                // Exercise system
                sut.Execute();
                // Verify outcome
                Assert.AreEqual(expectedOutput, sw.ToString(), "Execute");
                // Teardown
            }
        }
    }
}
