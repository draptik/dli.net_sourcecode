using System;
using System.Security.Principal;
using System.Collections.Generic;
namespace Ploeh.Samples.Commerce.Domain
{
    public partial interface IBasketService
    {
        void AddToBasket(int productId, int quantity, IPrincipal user);

        void Empty(IPrincipal user);

        Basket GetBasketFor(System.Security.Principal.IPrincipal user);
    }

    public partial interface IBasketService
    {
        IEnumerable<Basket> GetAllBaskets();
    }
}
