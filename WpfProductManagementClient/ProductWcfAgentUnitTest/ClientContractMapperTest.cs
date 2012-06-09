using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;
using Ploeh.SemanticComparison.Fluent;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class ClientContractMapperTest
    {
        [Fact]
        public void SutIsClientContractMapper()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Verify outcome
            Assert.IsAssignableFrom<IClientContractMapper>(sut);
            // Teardown
        }

        [Fact]
        public void MapNullMoneyContractWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((MoneyContract)null));
            // Teardown
        }

        [Fact]
        public void MapMoneyContractWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var contract = fixture.CreateAnonymous<MoneyContract>();
            var expectedVM = contract.AsSource().OfLikeness<MoneyViewModel>();

            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system
            var result = sut.Map(contract);
            // Verify outcome
            Assert.True(expectedVM.Equals(result));
            // Teardown
        }

        [Fact]
        public void MapNullProductContractWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((ProductContract)null));
            // Teardown
        }

        [Fact]
        public void MapProductContractWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var contract = fixture.CreateAnonymous<ProductContract>();
            var expectedVM = contract.AsSource().OfLikeness<ProductViewModel>()
                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice.AsSource().OfLikeness<MoneyViewModel>().Equals(d.UnitPrice))
                .Without(d => d.IsSelected);

            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system
            var result = sut.Map(contract);
            // Verify outcome
            Assert.True(expectedVM.Equals(result));
            // Teardown
        }

        [Fact]
        public void MapNullProductContractsWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((IEnumerable<ProductContract>)null).ToList());
            // Teardown
        }

        [Fact]
        public void MapProductContractsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var contracts = fixture.CreateMany<ProductContract>().ToList();
            var expectedVMs = (from c in contracts
                               select c.AsSource().OfLikeness<ProductViewModel>()
                                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.UnitPrice.AsSource().OfLikeness<MoneyViewModel>().Equals(d.UnitPrice))
                                .Without(d => d.IsSelected)).ToList();

            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system
            var result = sut.Map(contracts);
            // Verify outcome
            Assert.True(expectedVMs.Cast<object>().SequenceEqual(result.Cast<object>()));
            // Teardown
        }

        [Fact]
        public void MapNullProductEditorViewModelWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Map((ProductEditorViewModel)null));
            // Teardown
        }

        [Fact]
        public void MapProductEditorViewModelWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var vm = fixture.Build<ProductEditorViewModel>()
                .With(x => x.Price, fixture.CreateAnonymous<decimal>().ToString())
                .CreateAnonymous();
            var expectedContract = vm.AsSource().OfLikeness<ProductContract>()
                .With(d => d.UnitPrice).EqualsWhen((s, d) => s.Currency == d.UnitPrice.CurrencyCode && decimal.Parse(s.Price) == d.UnitPrice.Amount)
                .Without(d => d.ExtensionData);

            var sut = fixture.CreateAnonymous<ClientContractMapper>();
            // Exercise system
            var result = sut.Map(vm);
            // Verify outcome
            Assert.True(expectedContract.Equals(result));
            // Teardown
        }
    }
}
