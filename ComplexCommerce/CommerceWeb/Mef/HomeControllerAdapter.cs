using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using System.ComponentModel.Composition;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeControllerAdapter
    {
        private readonly ProductRepository repository;
        private readonly CurrencyProvider currencyProvider;

        [ImportingConstructor]
        public HomeControllerAdapter(ProductRepository repository, CurrencyProvider currencyProvider)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.repository = repository;
            this.currencyProvider = currencyProvider;
        }

        [Export]
        public HomeController HomeController 
        {
            get { return new HomeController(this.repository, this.currencyProvider); 
            }
        }
    }
}
