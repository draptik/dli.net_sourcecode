using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class OpenCircuitStateTest
    {
        [Fact]
        public void SutIsCircuitState()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<OpenCircuitState>();
            // Verify outcome
            Assert.IsAssignableFrom<ICircuitState>(sut);
            // Teardown
        }
    }
}
