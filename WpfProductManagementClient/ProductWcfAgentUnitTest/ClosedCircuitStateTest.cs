using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class ClosedCircuitStateTest
    {
        [Fact]
        public void SutIsCircuitState()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<ClosedCircuitState>();
            // Verify outcome
            Assert.IsAssignableFrom<ICircuitState>(sut);
            // Teardown
        }
    }
}
