using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class ContractMapperTest
    {
        [Theory, AutoMoqData]
        public void SutIsContractMapper(ContractMapper sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IContractMapper>(sut);
            // Teardown
        }
    }
}
