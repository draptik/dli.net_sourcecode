using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Ploeh.Samples.Menu.Mef
{
    public class SauceFilterCatalogFacts
    {
        [Fact]
        public void SutIsCatalog()
        {
            // Fixture setup
            var dummyCatalog = new AggregateCatalog();
            // Exercise system
            var sut = new SauceCatalog(dummyCatalog);
            // Verify outcome
            Assert.IsAssignableFrom<ComposablePartCatalog>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullCatalogThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SauceCatalog(null));
            // Teardown
        }
    }
}
