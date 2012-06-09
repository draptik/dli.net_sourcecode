using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.ServiceLocator
{
    public class ProductService
    {
        private readonly ProductRepository repository;

        public ProductService()
        {
            this.repository = Locator.GetService<ProductRepository>();
        }
    }
}
