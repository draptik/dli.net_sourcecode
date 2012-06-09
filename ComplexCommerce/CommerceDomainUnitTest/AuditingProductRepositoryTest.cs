using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class AuditingProductRepositoryTest
    {
        [Theory, AutoMoqData]
        public void SutIsProductRepository(AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ProductRepository>(sut);
            // Teardown
        }

        [Fact]
        public void GetFeaturedProductsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expectedProducts = fixture.CreateMany<Product>().ToList();

            fixture.Freeze<Mock<ProductRepository>>().Setup(r => r.GetFeaturedProducts()).Returns(expectedProducts);

            var sut = fixture.CreateAnonymous<AuditingProductRepository>();
            // Exercise system
            var result = sut.GetFeaturedProducts();
            // Verify outcome
            Assert.True(expectedProducts.SequenceEqual(result));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DeleteProductWillDeleteProductFromDecoratedRepository(int id, [Frozen]Mock<ProductRepository> repositoryMock, AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            sut.DeleteProduct(id);
            // Verify outcome
            repositoryMock.Verify(r => r.DeleteProduct(id));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DeleteProductWillAuditDeletion(int id, [Frozen]Mock<IAuditor> auditorMock, AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            sut.DeleteProduct(id);
            // Verify outcome
            auditorMock.Verify(a => a.Record(It.Is<AuditEvent>(ae => ae.Name == "ProductDeleted" && ae.Data.Equals(id))));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void InsertProductWillInsertProductInDecoratedRepository(Product product, [Frozen]Mock<ProductRepository> repositoryMock, AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            sut.InsertProduct(product);
            // Verify outcome
            repositoryMock.Verify(r => r.InsertProduct(product));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void InsertProductWillAuditInsertion(Product product, [Frozen]Mock<IAuditor> auditorMock, AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            sut.InsertProduct(product);
            // Verify outcome
            auditorMock.Verify(a => a.Record(It.Is<AuditEvent>(ae => ae.Name == "ProductInserted" && ae.Data.Equals(product))));
            // Teardown
        }

        [Fact]
        public void SelectProductWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var id = fixture.CreateAnonymous<int>();
            var expectedProduct = fixture.CreateAnonymous<Product>();

            fixture.Freeze<Mock<ProductRepository>>().Setup(r => r.SelectProduct(id)).Returns(expectedProduct);

            var sut = fixture.CreateAnonymous<AuditingProductRepository>();
            // Exercise system
            var result = sut.SelectProduct(id);
            // Verify outcome
            Assert.Equal(expectedProduct, result);
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var expectedProducts = fixture.CreateMany<Product>().ToList();
            fixture.Freeze<Mock<ProductRepository>>().Setup(r => r.SelectAllProducts()).Returns(expectedProducts);
            var sut = fixture.CreateAnonymous<AuditingProductRepository>();
            // Exercise system
            var result = sut.SelectAllProducts();
            // Verify outcome
            Assert.Equal(expectedProducts, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UpdateProductWillUpdateProductInDecoratedRepository(Product product, [Frozen]Mock<ProductRepository> repositoryMock, AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            sut.UpdateProduct(product);
            // Verify outcome
            repositoryMock.Verify(r => r.UpdateProduct(product));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UpdateProductWillAuditEvent(Product product, [Frozen]Mock<IAuditor> auditorMock, AuditingProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            sut.UpdateProduct(product);
            // Verify outcome
            auditorMock.Verify(a => a.Record(It.Is<AuditEvent>(ae => ae.Name == "ProductUpdated" && ae.Data.Equals(product))));
            // Teardown
        }
    }
}
