using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.QualityTools.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Moq;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    [TestClass]
    public class EvaluatedProductTest
    {
        public EvaluatedProductTest()
        {
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullNameWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture();
            string nullName = null;
            // Exercise system
            fixture.Build<EvaluatedProduct>()
                .WithConstructor((int id, Money unitPrice) => new EvaluatedProduct(id, nullName, unitPrice))
                .CreateAnonymous();
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void IdWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            int expectedId = fixture.CreateAnonymous<int>();
            var sut = fixture.Build<EvaluatedProduct>()
                .WithConstructor((string name, Money unitPrice) => new EvaluatedProduct(expectedId, name, unitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            int result = sut.Id;
            // Verify outcome
            Assert.AreEqual<int>(expectedId, result, "Id");
            // Teardown
        }

        [TestMethod]
        public void NameWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            string expectedName = fixture.CreateAnonymous("Name");
            var sut = fixture.Build<EvaluatedProduct>()
                .WithConstructor((int id, Money unitPrice) => new EvaluatedProduct(id, expectedName, unitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            string result = sut.Name;
            // Verify outcome
            Assert.AreEqual<string>(expectedName, result, "Name");
            // Teardown
        }

        [TestMethod]
        public void UnitPriceWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedUnitPrice = fixture.CreateAnonymous<Money>();
            var sut = fixture.Build<EvaluatedProduct>()
                .WithConstructor((int id, string name) => new EvaluatedProduct(id, name, expectedUnitPrice))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            Money result = sut.UnitPrice;
            // Verify outcome
            Assert.AreEqual<Money>(expectedUnitPrice, result, "UnitPrice");
            // Teardown
        }

        [TestMethod]
        public void SutIsValuableItem()
        {
            // Fixture setup
            var fixture = new Fixture();
            Type expectedInterface = typeof(IValuableItem<EvaluatedProduct>);
            // Exercise system
            var sut = fixture.CreateAnonymous<EvaluatedProduct>();
            // Verify outcome
            Assert.IsInstanceOfType(sut, expectedInterface);
            // Teardown
        }

        [TestMethod]
        public void ValueIsUnitPrice()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<EvaluatedProduct>();
            var expectedValue = sut.UnitPrice;
            // Exercise system
            var result = sut.Value;
            // Verify outcome
            Assert.AreEqual<Money>(expectedValue, result, "Value");
            // Teardown
        }

        [TestMethod]
        public void WithUnitPriceWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<EvaluatedProduct>();
            var newUnitPrice = fixture.CreateAnonymous<Money>();
            var expectedResult = new Likeness(new
                {
                    sut.Id,
                    sut.Name,
                    UnitPrice = newUnitPrice
                });
            // Exercise system
            EvaluatedProduct result = sut.WithUnitPrice(newUnitPrice);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "WithUnitPrice");
            // Teardown
        }

        [TestMethod]
        public void ConvertWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var currency = fixture.CreateAnonymous<Currency>();
            var sut = fixture.CreateAnonymous<EvaluatedProduct>();
            var expectedProduct = new Likeness(sut.WithUnitPrice(sut.UnitPrice.ConvertTo(currency)));
            // Exercise system
            EvaluatedProduct result = sut.ConvertTo(currency);
            // Verify outcome
            Assert.AreEqual(expectedProduct, result, "ConvertTo");
            // Teardown
        }
    }
}
