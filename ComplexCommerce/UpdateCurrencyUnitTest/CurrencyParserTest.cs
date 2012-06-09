using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine.UnitTest
{
    [TestClass]
    public class CurrencyParserTest
    {
        public CurrencyParserTest()
        {
        }

        [TestMethod]
        public void ParseNullWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            ICommand result = sut.Parse(null);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(HelpCommand));
            // Teardown
        }

        [TestMethod]
        public void ParseEmptyArgsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(Enumerable.Empty<string>());
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(HelpCommand));
            // Teardown
        }

        [TestMethod]
        public void ParseSingleArgWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var arg = "DKK";
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(new[] { arg });
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(HelpCommand));
            // Teardown
        }

        [TestMethod]
        public void ParseTwoArgsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var args = fixture.CreateMany<string>(2).ToList();
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(args);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(HelpCommand));
            // Teardown
        }

        [TestMethod]
        public void ParseThreeInvalidArgsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var args = fixture.CreateMany<string>().ToList();
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(args);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(HelpCommand));
            // Teardown
        }

        [TestMethod]
        public void ParseThreeOrMoreArgumentsWhenThirdIsInvalidWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var args = fixture.CreateMany<string>().ToList();
            args[0] = "euR";
            args[1] = "DkK";
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(args);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(HelpCommand));
            // Teardown
        }

        [TestMethod]
        public void ParseValidArgsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
                        
            var rate = fixture.CreateAnonymous<decimal>();

            var srcCurrencyArg = "EUR";
            var destCurrencyArg = "USD";
            var rateArg = rate.ToString();

            var currencyStub = fixture.CreateMoq<Currency>();
            currencyStub.Setup(c => c.Code).Returns(srcCurrencyArg);

            fixture.FreezeMoq<CurrencyProvider>().Setup(cp => cp.GetCurrency(srcCurrencyArg)).Returns(currencyStub.Object);

            var expectedResult = new Likeness<CurrencyUpdateCommand, CurrencyUpdateCommand>(new CurrencyUpdateCommand(currencyStub.Object, destCurrencyArg, rate));

            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(new[] { destCurrencyArg, srcCurrencyArg, rateArg });
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Parse");
            // Teardown
        }
    }
}
