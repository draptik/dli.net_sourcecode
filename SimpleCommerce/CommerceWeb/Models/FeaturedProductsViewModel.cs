using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ploeh.Samples.Commerce.Web.Models
{
    public class FeaturedProductsViewModel
    {
        private List<ProductViewModel> products;

        public FeaturedProductsViewModel()
        {
            this.products = new List<ProductViewModel>();
        }

        public IList<ProductViewModel> Products
        {
            get { return this.products; }
        }
    }
}
