using System;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class WcfProductManagementAgentTest
    {
        [Fact]
        public void SutIsProductManagementAgent()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Verify outcome
            Assert.IsAssignableFrom<IProductManagementAgent>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullFactoryWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var dummyMapper = fixture.CreateAnonymous<IClientContractMapper>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new WcfProductManagementAgent(null, dummyMapper));
            // Teardown
        }

        [Fact]
        public void CreateWithNullMapperWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var dummyFactory = fixture.CreateAnonymous<IProductChannelFactory>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new WcfProductManagementAgent(dummyFactory, null));
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var contracts = fixture.CreateMany<ProductContract>().ToArray();
            var productsVMs = fixture.CreateMany<ProductViewModel>().ToList();

            fixture.Freeze<Mock<IProductChannelFactory>>().Setup(f => f.CreateChannel()).Returns(() =>
                {
                    var chStub = fixture.CreateAnonymous<Mock<IProductManagementServiceChannel>>();
                    chStub.Setup(c => c.SelectAllProducts()).Returns(contracts);
                    return chStub.Object;
                });
            fixture.Freeze<Mock<IClientContractMapper>>().Setup(m => m.Map(contracts)).Returns(productsVMs);

            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Exercise system
            var result = sut.SelectAllProducts();
            // Verify outcome
            Assert.True(productsVMs.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InsertNullProductWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.InsertProduct(null));
            // Teardown
        }

        [Fact]
        public void InsertProductWillInsertCorrectlyInChannel()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var vm = fixture.CreateAnonymous<ProductEditorViewModel>();
            var contract = fixture.CreateAnonymous<ProductContract>();

            var channelMock = fixture.CreateAnonymous<Mock<IProductManagementServiceChannel>>();
            fixture.Freeze<Mock<IProductChannelFactory>>().Setup(f => f.CreateChannel()).Returns(channelMock.Object);
            fixture.Freeze<Mock<IClientContractMapper>>().Setup(m => m.Map(vm)).Returns(contract);

            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Exercise system
            sut.InsertProduct(vm);
            // Verify outcome
            channelMock.Verify(c => c.InsertProduct(contract));
            // Teardown
        }

        [Fact]
        public void UpdateNullProductWillThrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.UpdateProduct(null));
            // Teardown
        }

        [Fact]
        public void UpdateProductWillUpdateCorrectlyInChannel()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var vm = fixture.CreateAnonymous<ProductEditorViewModel>();
            var contract = fixture.CreateAnonymous<ProductContract>();

            var channelMock = fixture.CreateAnonymous<Mock<IProductManagementServiceChannel>>();
            fixture.Freeze<Mock<IProductChannelFactory>>().Setup(f => f.CreateChannel()).Returns(channelMock.Object);
            fixture.Freeze<Mock<IClientContractMapper>>().Setup(m => m.Map(vm)).Returns(contract);

            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Exercise system
            sut.UpdateProduct(vm);
            // Verify outcome
            channelMock.Verify(c => c.UpdateProduct(contract));
            // Teardown
        }

        [Fact]
        public void DeleteProductWillDeleteFromChannel()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var id = fixture.CreateAnonymous<int>();

            var channelMock = fixture.CreateAnonymous<Mock<IProductManagementServiceChannel>>();
            fixture.Freeze<Mock<IProductChannelFactory>>().Setup(f => f.CreateChannel()).Returns(channelMock.Object);

            var sut = fixture.CreateAnonymous<WcfProductManagementAgent>();
            // Exercise system
            sut.DeleteProduct(id);
            // Verify outcome
            channelMock.Verify(c => c.DeleteProduct(id));
            // Teardown
        }
    }
}
