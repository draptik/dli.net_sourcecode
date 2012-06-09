using System;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Moq;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.CommerceService.UnitTest
{
    public class ProductManagementServiceTest
    {
        [Theory, AutoMoqData]
        public void SutIsProductManagementService(ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IProductManagementService>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CreateWithNullProductRepositoryWillThrow(IContractMapper mapper)
        {
            // Fixture setup
            ProductRepository nullRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ProductManagementService(nullRepository, mapper));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void CreateWithNullContractMapperWillThrow(ProductRepository repository)
        {
            // Fixture setup
            IContractMapper nullMapper = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ProductManagementService(repository, nullMapper));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ProductRepositoryIsCorrect([Frozen]ProductRepository expectedRepository, ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            ProductRepository result = sut.ProductRepository;
            // Verify outcome
            Assert.Equal(expectedRepository, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void MapperIsCorrect([Frozen]IContractMapper expectedMapper, ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            IContractMapper result = sut.Mapper;
            // Verify outcome
            Assert.Equal(expectedMapper, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DeleteProductWillDeleteProductFromRepository(int id, [Frozen]Mock<ProductRepository> repMock, ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            sut.DeleteProduct(id);
            // Verify outcome
            repMock.Verify(r => r.DeleteProduct(id));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void InsertNullProductWillThrow(ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.InsertProduct(null));
            // Teardown
        }

        [Fact]
        public void InsertNormalProductWillInsertProductCorrectlyIntoRepository()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var productContract = fixture.CreateAnonymous<ProductContract>();
            var product = fixture.CreateAnonymous<Product>();

            fixture.Freeze<Mock<IContractMapper>>().Setup(m => m.Map(productContract)).Returns(product);
            var repMock = fixture.Freeze<Mock<ProductRepository>>();

            var sut = fixture.CreateAnonymous<ProductManagementService>();
            // Exercise system
            sut.InsertProduct(productContract);
            // Verify outcome
            repMock.Verify(r => r.InsertProduct(product));
            // Teardown
        }

        [Fact]
        public void SelectProductWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var id = fixture.CreateAnonymous<int>();
            var product = fixture.CreateAnonymous<Product>();
            var expectedContract = fixture.CreateAnonymous<ProductContract>();

            fixture.Freeze<Mock<ProductRepository>>().Setup(r => r.SelectProduct(id)).Returns(product);
            fixture.Freeze<Mock<IContractMapper>>().Setup(m => m.Map(product)).Returns(expectedContract);

            var sut = fixture.CreateAnonymous<ProductManagementService>();
            // Exercise system
            var result = sut.SelectProduct(id);
            // Verify outcome
            Assert.Equal(expectedContract, result);
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var products = fixture.CreateMany<Product>().ToList();
            var expectedContracts = fixture.CreateMany<ProductContract>().ToList();

            fixture.Freeze<Mock<ProductRepository>>().Setup(r => r.SelectAllProducts()).Returns(products);
            fixture.Freeze<Mock<IContractMapper>>().Setup(m => m.Map(products)).Returns(expectedContracts);

            var sut = fixture.CreateAnonymous<ProductManagementService>();
            // Exercise system
            var result = sut.SelectAllProducts();
            // Verify outcome
            Assert.True(expectedContracts.SequenceEqual(result), "SelectAllProducts");
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UpdateNullProductWillThrow(ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.UpdateProduct(null));
            // Teardown
        }

        [Fact]
        public void UpdateProductWillUpdateCorrectlyInRepository()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var contract = fixture.CreateAnonymous<ProductContract>();
            var product = fixture.CreateAnonymous<Product>();

            fixture.Freeze<Mock<IContractMapper>>().Setup(m => m.Map(contract)).Returns(product);
            var repMock = fixture.Freeze<Mock<ProductRepository>>();

            var sut = fixture.CreateAnonymous<ProductManagementService>();
            // Exercise system
            sut.UpdateProduct(contract);
            // Verify outcome
            repMock.Verify(r => r.UpdateProduct(product));
            // Teardown
        }
    }
}
