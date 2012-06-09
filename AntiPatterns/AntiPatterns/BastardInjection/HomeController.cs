using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.BastardInjection
{
    public class HomeController
    {
        public object Index()
        {
            var productService = new ProductService();

            return new object();
        }
    }
}
