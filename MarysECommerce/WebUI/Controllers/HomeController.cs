using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ploeh.Samples.Mary.ECommerce.Domain;
using System.Security.Principal;

namespace Ploeh.Samples.Mary.ECommerce.WebUI.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            bool isPreferredCustomer = 
                this.User.IsInRole("PreferredCustomer");

            var service = new ProductService();
            var products = 
                service.GetFeaturedProducts(isPreferredCustomer);
            this.ViewData["Products"] = products;

            return this.View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
