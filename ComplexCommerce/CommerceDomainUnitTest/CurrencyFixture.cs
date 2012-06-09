using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Moq;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    internal class CurrencyFixture : Fixture
    {
        public CurrencyFixture()
        {
            this.Register(() =>
                {
                    var currencyStub = new Mock<Currency>();
                    currencyStub.Setup(c => c.GetExchangeRateFor(It.IsAny<string>())).Returns(this.CreateAnonymous<decimal>());
                    currencyStub.SetupGet(c => c.Code).Returns(this.CreateAnonymous("CurrencyCode"));
                    return currencyStub.Object;
                });
        }
    }
}
