using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison;
using Xunit;
using Moq;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.UpdateCurrency.ApplicationServices.UnitTest
{
    public class CurrencyParserTest
    {
        [Theory, AutoMoqData]
        public void ParseNullWillReturnCorrectResult(CurrencyParser sut)
        {
            // Fixture setup
            // Exercise system
            ICommand result = sut.Parse(null);
            // Verify outcome
            Assert.IsAssignableFrom<HelpCommand>(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ParseEmptyArgsWillReturnCorrectResult(CurrencyParser sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.Parse(Enumerable.Empty<string>());
            // Verify outcome
            Assert.IsAssignableFrom<HelpCommand>(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ParseSingleArgWillReturnCorrectResult(CurrencyParser sut)
        {
            // Fixture setup
            var arg = "DKK";
            // Exercise system
            var result = sut.Parse(new[] { arg });
            // Verify outcome
            Assert.IsAssignableFrom<HelpCommand>(result);
            // Teardown
        }

        [Fact]
        public void ParseTwoArgsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var args = fixture.CreateMany<string>(2).ToList();
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(args);
            // Verify outcome
            Assert.IsAssignableFrom<HelpCommand>(result);
            // Teardown
        }

        [Fact]
        public void ParseThreeInvalidArgsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var args = fixture.CreateMany<string>().ToList();
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(args);
            // Verify outcome
            Assert.IsAssignableFrom<HelpCommand>(result);
            // Teardown
        }

        [Fact]
        public void ParseThreeOrMoreArgumentsWhenThirdIsInvalidWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var args = fixture.CreateMany<string>().ToList();
            args[0] = "euR";
            args[1] = "DkK";
            var sut = fixture.CreateAnonymous<CurrencyParser>();
            // Exercise system
            var result = sut.Parse(args);
            // Verify outcome
            Assert.IsAssignableFrom<HelpCommand>(result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ParseValidArgsWillReturnCorrectResult(decimal rate, Mock<Currency> currencyStub, [Frozen]Mock<CurrencyProvider> currencyProviderStub, CurrencyParser sut)
        {
            // Fixture setup
            var srcCurrencyArg = "EUR";
            var destCurrencyArg = "USD";
            var rateArg = rate.ToString();

            currencyStub.Setup(c => c.Code).Returns(srcCurrencyArg);

            currencyProviderStub.Setup(cp => cp.GetCurrency(srcCurrencyArg)).Returns(currencyStub.Object);

            var expectedResult = new Likeness<CurrencyUpdateCommand, CurrencyUpdateCommand>(new CurrencyUpdateCommand(currencyStub.Object, destCurrencyArg, rate));
            // Exercise system
            var result = sut.Parse(new[] { destCurrencyArg, srcCurrencyArg, rateArg });
            // Verify outcome
            Assert.True(expectedResult.Equals(result));
            // Teardown
        }
    }
}
