using System.Collections.Generic;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract partial class BasketRepository
    {
        public abstract void AddToBasket(int productId, int quantity, IPrincipal user);

        public abstract void Empty(IPrincipal user);

        public abstract IEnumerable<Extent> GetBasketFor(IPrincipal user);
    }

    public abstract partial class BasketRepository
    {
        public abstract IEnumerable<Basket> GetAllBaskets();
    }
}
