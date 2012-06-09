using System;
using System.Web.Mvc;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Models;

namespace Ploeh.Samples.Lifetime.MvcApplication.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly DiscountCampaign campaign;
        private readonly BasketDiscountPolicy policy;

        public HomeController(DiscountCampaign campaign,
            BasketDiscountPolicy policy)
        {
            if (campaign == null)
            {
                throw new ArgumentNullException("campaign");
            }
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }

            this.campaign = campaign;
            this.policy = policy;
        }

        public DiscountCampaign Campaign
        {
            get { return this.campaign; ; }
        }

        public BasketDiscountPolicy Policy
        {
            get { return this.policy; }
        }

        public ViewResult Index()
        {
            this.campaign.AddProduct(new Product { Name = "Success" });

            var vm = new HomeIndexViewModel();

            var products = this.policy.GetDiscountedProducts();
            foreach (var p in products)
            {
                vm.Products.Add(p);
            }

            return this.View(vm);
        }

        public ActionResult About()
        {
            return this.View();
        }
    }
}
