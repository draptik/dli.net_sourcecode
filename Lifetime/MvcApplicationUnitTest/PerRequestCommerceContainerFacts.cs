using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Lifetime.MvcApplication.UnitTest
{
    public class PerRequestCommerceContainerFacts
    {
        [Theory, AutoMvcData]
        public void SutIsCommerceContainer(PerRequestCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ICommerceContainer>(sut);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolveHomeControllerReturnsCorrectResult(PerRequestCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            Assert.IsAssignableFrom<HomeController>(result);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ResolvedHomeControllerHasCorrectCampaignRepository(PerRequestCommerceContainer sut)
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
        public void ResolvedHomeControllerHasCorrectPolicy(PerRequestCommerceContainer sut)
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
        public void ResolvedHomeControllerHasCorrectPolicyRepository(PerRequestCommerceContainer sut)
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
        public void ResolvedRepositoriesAreDifferent(PerRequestCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.ResolveHomeController();
            // Verify outcome
            var homeController = Assert.IsAssignableFrom<HomeController>(result);
            var policy = Assert.IsAssignableFrom<RepositoryBasketDiscountPolicy>(homeController.Policy);
            Assert.Same(homeController.Campaign.Repository, policy.Repository);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void RepositoriesDifferAcrossRequests(PerRequestCommerceContainer sut)
        {
            // Fixture setup
            // Exercise system
            var result1 = sut.ResolveHomeController();
            var result2 = sut.ResolveHomeController();
            // Verify outcome
            var homeController1 = Assert.IsAssignableFrom<HomeController>(result1);
            var policy1 = Assert.IsAssignableFrom<RepositoryBasketDiscountPolicy>(homeController1.Policy);

            var homeController2 = Assert.IsAssignableFrom<HomeController>(result2);
            var policy2 = Assert.IsAssignableFrom<RepositoryBasketDiscountPolicy>(homeController2.Policy);

            Assert.NotSame(homeController1.Campaign.Repository, homeController2.Campaign.Repository);
            Assert.NotSame(policy1.Repository, policy2.Repository);
            Assert.NotSame(homeController1.Campaign.Repository, policy2.Repository);
            Assert.NotSame(homeController2.Campaign.Repository, policy1.Repository);
            // Teardown
        }
    }
}
