using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Dependency.Lifetime;

namespace Ploeh.Samples.Lifetime.MvcApplication.Models
{
    public class HomeIndexViewModel
    {
        private readonly List<Product> products;

        public HomeIndexViewModel()
        {
            this.products = new List<Product>();
        }

        public IList<Product> Products
        {
            get { return this.products; }
        }
    }
}
