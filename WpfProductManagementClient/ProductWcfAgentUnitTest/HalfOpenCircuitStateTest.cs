using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class HalfOpenCircuitStateTest
    {
        [Fact]
        public void SutIsCurcuitState()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<HalfOpenCircuitState>();
            // Verify outcome
            Assert.IsAssignableFrom<ICircuitState>(sut);
            // Teardown
        }
    }
}
