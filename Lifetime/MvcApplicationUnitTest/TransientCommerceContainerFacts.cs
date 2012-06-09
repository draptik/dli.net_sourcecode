using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Lifetime.MvcApplication.UnitTest
{
    public class TransientCommerceContainerFacts
    {
        [Theory, AutoMvcData]
        public void SutIsCommerceContainer(TransientCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ICommerceContainer>(sut);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolveHomeControllerReturnsCorrectResult(TransientCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            Assert.IsAssignableFrom<HomeController>(result);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolvedHomeControllerHasCorrectCampaignRepository(TransientCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            var homeController = Assert.IsAssignableFrom<HomeController>(result);
            Assert.IsAssignableFrom<SqlDiscountRepository>(homeController.Campaign.Repository);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolvedHomeControllerHasCorrectPolicy(TransientCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            var homeController = Assert.IsAssignableFrom<HomeController>(result);
            Assert.IsAssignableFrom<RepositoryBasketDiscountPolicy>(homeController.Policy);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolvedHomeControllerHasCorrectPolicyRepository(TransientCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            var homeController = Assert.IsAssignableFrom<HomeController>(result);
            var policy = Assert.IsAssignableFrom<RepositoryBasketDiscountPolicy>(homeController.Policy);
            Assert.IsAssignableFrom<SqlDiscountRepository>(policy.Repository);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolvedRepositoriesAreDifferent(TransientCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            var homeController = Assert.IsAssignableFrom<HomeController>(result);
            var policy = Assert.IsAssignableFrom<RepositoryBasketDiscountPolicy>(homeController.Policy);
            Assert.NotSame(homeController.Campaign.Repository, policy.Repository);
            // Teardown
        }
    }
}
