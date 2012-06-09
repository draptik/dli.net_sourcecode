using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;
using System.Configuration;

namespace Ploeh.Samples.Lifetime.MvcApplication
{
	public partial class CommerceContainer
	{
		public IController ResolveHomeController()
		{
			var discountPolicy =
				new RepositoryBasketDiscountPolicy(
					this.ResolveDiscountRepository());

			var campaign = new DiscountCampaign(
				this.ResolveDiscountRepository());

			return new HomeController(
				campaign, discountPolicy);
		}

		protected virtual DiscountRepository ResolveDiscountRepository()
		{
			var repository = HttpContext.Current
				.Items["DiscountRepository"]
				as DiscountRepository;
			if (repository == null)
			{
				var connStr = ConfigurationManager
					.ConnectionStrings["CommerceObjectContext"]
					.ConnectionString;
				repository = new SqlDiscountRepository(connStr);
				HttpContext.Current
					.Items["DiscountRepository"] = repository;
			}

			return repository;
		}
	}

	public partial class CommerceContainer : ICommerceContainer
	{
	}
}
