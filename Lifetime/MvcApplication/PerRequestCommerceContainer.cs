using System.Configuration;
using System.Web.Mvc;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;

namespace Ploeh.Samples.Lifetime.MvcApplication
{
	public class PerRequestCommerceContainer : ICommerceContainer
	{
		#region ICommerceContainer Members

		public IController ResolveHomeController()
		{
			var connStr = ConfigurationManager
				.ConnectionStrings["CommerceObjectContext"]
				.ConnectionString;
			var repository =
				new SqlDiscountRepository(connStr);

			var discountCampaign =
				new DiscountCampaign(repository);
			var discountPolicy =
				new RepositoryBasketDiscountPolicy(repository);

			return new HomeController(discountCampaign, discountPolicy);
		}

		#endregion
	}
}
