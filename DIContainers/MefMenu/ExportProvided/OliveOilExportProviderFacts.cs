using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Moq;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.ExportProvided
{
    public class OliveOilExportProviderFacts
    {
        [Fact]
        public void SutIsExportProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = new OliveOilExportProvider();
            // Verify outcome
            Assert.IsAssignableFrom<ExportProvider>(sut);
            // Teardown
        }

        [Fact]
        public void GetExportsReturnsCorrectResultWhenImportDefinitionIsNotOliveOil()
        {
            // Fixture setup
            var importStub = new Mock<ImportDefinition>();
            importStub.SetupGet(id => id.ContractName).Returns("Foo");

            var sut = new OliveOilExportProvider();
            // Exercise system
            var result = sut.GetExports(importStub.Object);
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }

        [Fact]
        public void GetExportsReturnsCorrectResultWhenImportDefinitionIsOliveOil()
        {
            // Fixture setup
            var importStub = new Mock<ImportDefinition>();
            importStub.SetupGet(id => id.ContractName).Returns(typeof(OliveOil).FullName);

            var sut = new OliveOilExportProvider();
            // Exercise system
            var result = sut.GetExports(importStub.Object);
            // Verify outcome
            Assert.True(result.Any(x => x.Value is OliveOil));
            // Teardown
        }
    }
}
