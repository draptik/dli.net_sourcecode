using System;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Ploeh.Samples.AssortedCodeSnippets;
using Ploeh.Samples.AssortedCodeSnippets.Decorator.Security.Assertive;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.Decorator.Security.Assertive
{
    public class SecureProductRepositoryTest : IDisposable
    {
        private IPrincipal originalPrincipal;

        public SecureProductRepositoryTest()
        {
            this.originalPrincipal = Thread.CurrentPrincipal;
        }

        [Theory, AutoMoqData]
        public void SutIsProductRepository(SecureProductRepository sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ProductRepository>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void InsertProductWillThrowWhenPrincipalIsNotInRequiredRole(SecureProductRepository sut, Product p)
        {
            // Fixture setup
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            // Exercise system and verify outcome
            Assert.Throws<SecurityException>(() =>
                sut.InsertProduct(p));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void InsertProductWillInsertProductIntoDecoratedRepositoryWhenPrincipalIsInCorrectRole([Frozen]Mock<ProductRepository> repositoryMock,
            GenericIdentity id, Product product, SecureProductRepository sut)
        {
            // Fixture setup
            Thread.CurrentPrincipal = new GenericPrincipal(id, new[] { "ProductManager" });
            // Exercise system
            sut.InsertProduct(product);
            // Verify outcome
            repositoryMock.Verify(r => r.InsertProduct(product));
            // Teardown
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread.CurrentPrincipal = this.originalPrincipal;
        }

        #endregion
    }
}
