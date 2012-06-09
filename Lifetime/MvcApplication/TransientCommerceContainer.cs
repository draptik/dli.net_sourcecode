using System.Configuration;
using System.Web.Mvc;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;

namespace Ploeh.Samples.Lifetime.MvcApplication
{
    public class TransientCommerceContainer : ICommerceContainer
    {
        #region ICommerceContainer Members

        public IController ResolveHomeController()
        {
            var connStr = ConfigurationManager
                .ConnectionStrings["CommerceObjectContext"]
                .ConnectionString;

            var discountCampaign = 
                new DiscountCampaign(
                    new SqlDiscountRepository(connStr));
            var discountPolicy = 
                new RepositoryBasketDiscountPolicy(
                    new SqlDiscountRepository(connStr));

            return new HomeController(
                discountCampaign, discountPolicy);
        }

        #endregion
    }
}
