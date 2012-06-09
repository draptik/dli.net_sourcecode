using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class Scenario
    {
        [Theory, AutoMoqData]
        public void OriginalCommerceExampleFragment(string connectionString)
        {
            var discountRepository =
                new SqlDiscountRepository(connectionString);

            var discountPolicy =
                new RepositoryBasketDiscountPolicy(
                    discountRepository);
        }

        [Theory, AutoMoqData]
        public void TwoConsumersRequiringTheSameDependencyButConsumingSeperateInstance(string connectionString)
        {
            var repositoryForPolicy =
                new SqlDiscountRepository(connectionString);
            var repositoryForCampaign =
                new SqlDiscountRepository(connectionString);

            var discountPolicy =
                new RepositoryBasketDiscountPolicy(
                    repositoryForPolicy);

            var campaign =
                new DiscountCampaign(repositoryForCampaign);
        }

        [Theory, AutoMoqData]
        public void TwoConsumersSharingOneDependency(string connectionString)
        {
            var repository =
                new SqlDiscountRepository(connectionString);

            var discountPolicy =
                new RepositoryBasketDiscountPolicy(repository);

            var campaign = new DiscountCampaign(repository);
        }
    }
}
