using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class ProductManagementServiceTest
    {
        [Theory, AutoMoqData]
        public void SutIsProductManagementService(ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IProductManagementService>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RepositoryIsCorrect([Frozen]ProductRepository expectedRepository, ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            ProductRepository result = sut.Repository;
            // Verify outcome
            Assert.Equal(expectedRepository, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ContractMapperIsCorrect([Frozen]IContractMapper expectedMapper, ProductManagementService sut)
        {
            // Fixture setup
            // Exercise system
            IContractMapper result = sut.ContractMapper;
            // Verify outcome
            Assert.Equal(expectedMapper, result);
            // Teardown
        }
    }
}
