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
    public class BasketControllerAdapter
    {
        private readonly IBasketService basketService;
        private readonly CurrencyProvider currencyProvider;

        [ImportingConstructor]
        public BasketControllerAdapter(IBasketService basketService, CurrencyProvider currencyProvider)
        {
            if (basketService == null)
            {
                throw new ArgumentNullException("basketService");
            }
            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.basketService = basketService;
            this.currencyProvider = currencyProvider;
        }

        [Export]
        public BasketController BasketController
        {
            get { return new BasketController(this.basketService, this.currencyProvider); }
        }
    }
}
