using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.Samples.DI.AntiPatterns.ServiceLocator;
using Ploeh.Samples.DI.AntiPatterns;
using Moq;

namespace Ploeh.Samples.DI.AntiPatternsTest.ServiceLocator
{
    public class ProductServiceTest : IDisposable
    {
        [Fact]
        public void CreateWithConfigureServiceLocatorWillSucceed()
        {
            // Fixture setup
            var stub = new Mock<ProductRepository>().Object;
            Locator.Register<ProductRepository>(stub);
            // Exercise system
            new ProductService();
            // Verify outcome
            // Teardown
        }

        [Fact]
        public void CreateWithUnconfigureServiceLocatorWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<KeyNotFoundException>(() => new ProductService());
            // Teardown
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Locator.Reset();
            }
        }
    }
}
