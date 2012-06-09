using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine.UnitTest
{
    [TestClass]
    public class CurrencyContainerTest
    {
        public CurrencyContainerTest()
        {
        }

        [TestMethod]
        public void ResolveCurrencyParserWillReturnInstance()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var sut = fixture.CreateAnonymous<CurrencyContainer>();
            // Exercise system
            CurrencyParser result = sut.ResolveCurrencyParser();
            // Verify outcome
            Assert.IsNotNull(result, "ResolveCurrencyParser");
            // Teardown
        }
    }
}
