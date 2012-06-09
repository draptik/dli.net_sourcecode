using System.Runtime.Serialization;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    internal class WcfFixture : Fixture
    {
        internal WcfFixture()
        {
            this.Customize(new AutoMoqCustomization());

            this.Inject<ExtensionDataObject>(null);
        }
    }
}
