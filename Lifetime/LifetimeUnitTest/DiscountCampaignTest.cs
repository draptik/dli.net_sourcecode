using Moq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class DiscountCampaignTest
    {
        [Theory, AutoMoqData]
        public void AddProductWillAddProductToRepository(Product product, [Frozen]Mock<DiscountRepository> repositoryMock, DiscountCampaign sut)
        {
            // Fixture setup
            // Exercise system
            sut.AddProduct(product);
            // Verify outcome
            repositoryMock.Verify(r => r.Products.Add(product));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void RepositoryIsCorrect([Frozen]DiscountRepository expectedRepository, DiscountCampaign sut)
        {
            // Fixture setup
            // Exercise system
            DiscountRepository result = sut.Repository;
            // Verify outcome
            Assert.Equal(expectedRepository, result);
            // Teardown
        }
    }
}
