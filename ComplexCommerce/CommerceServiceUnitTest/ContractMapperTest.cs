using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.SemanticComparison.Fluent;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.CommerceService.UnitTest
{
    public class ContractMapperTest
    {
        [Theory, AutoMoqData]
        public void SutIsContractMapper(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IContractMapper>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullMoneyWillThrow(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((Money)null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapMoneyWillReturnCorrectResult(Money money, ContractMapper sut)
        {
            // Fixture setup
            var expectedContract = money.AsSource().OfLikeness<MoneyContract>();
            // Exercise system
            MoneyContract result = sut.Map(money);
            // Verify outcome
            Assert.True(expectedContract.Equals(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullMoneyContractWillThrow(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((MoneyContract)null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapMoneyContractWillReturnCorrectResult(MoneyContract contract, ContractMapper sut)
        {
            // Fixture setup
            var expectedMoney = new Money(contract.Amount, contract.CurrencyCode);
            // Exercise system
            Money result = sut.Map(contract);
            // Verify outcome
            Assert.Equal(expectedMoney, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullProductWillThrow(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((Product)null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapProductWillReturnCorrectResult(Product product, ContractMapper sut)
        {
            // Fixture setup
            var expectedContract = product.AsSource().OfLikeness<ProductContract>()
                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice.AsSource().OfLikeness<MoneyContract>().Equals(d.UnitPrice));
            // Exercise system
            ProductContract result = sut.Map(product);
            // Verify outcome
            Assert.True(expectedContract.Equals(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullProductsWillThrow(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((IEnumerable<Product>)null).ToList());
            // Teardown
        }

        [Fact]
        public void MapProductsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var products = fixture.CreateMany<Product>().ToList();
            var expectedContracts = (from p in products
                                     select p.AsSource().OfLikeness<ProductContract>()
                                        .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice.AsSource().OfLikeness<MoneyContract>().Equals(d.UnitPrice))).ToList();

            var sut = fixture.CreateAnonymous<ContractMapper>();
            // Exercise system
            IEnumerable<ProductContract> result = sut.Map(products);
            // Verify outcome
            Assert.True(expectedContracts.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapNullProductContractWillThrow(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((ProductContract)null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapProductContractWillReturnCorrectResult(ProductContract contract, ContractMapper sut)
        {
            // Fixture setup
            var expectedProduct = contract.AsSource().OfLikeness<Product>()
                .With(d => d.UnitPrice).EqualsWhen((s, d) => new Money(s.UnitPrice.Amount, s.UnitPrice.CurrencyCode).Equals(d.UnitPrice));
            // Exercise system
            Product result = sut.Map(contract);
            // Verify outcome
            Assert.True(expectedProduct.Equals(result));
            // Teardown
        }
    }
}
