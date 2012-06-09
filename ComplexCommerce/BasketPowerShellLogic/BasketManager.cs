using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.BasketPowerShellModel
{
    public class BasketManager
    {
        private readonly IBasketService basketService;

        public BasketManager(IBasketService basketService)
        {
            if (basketService == null)
            {
                throw new ArgumentNullException("basketService");
            }

            this.basketService = basketService;
        }

        public IEnumerable<BasketView> GetAllBaskets()
        {
            return from b in this.basketService.GetAllBaskets()
                   select new BasketView(b);
        }

        public void RemoveBasket(string owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            var p = new GenericPrincipal(new GenericIdentity(owner), null);
            this.basketService.Empty(p);
        }
    }
}
