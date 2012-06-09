using System;
using System.Security.Principal;
using System.Collections.Generic;

namespace Ploeh.Samples.Commerce.Domain
{
    public partial class BasketService : IBasketService
    {
        private readonly BasketRepository repository;
        private readonly BasketDiscountPolicy discountPolicy;

        public BasketService(BasketRepository repository,
            BasketDiscountPolicy discountPolicy)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (discountPolicy == null)
            {
                throw new ArgumentNullException("discountPolicy");
            }
            
            this.repository = repository;
            this.discountPolicy = discountPolicy;
        }

        public void AddToBasket(int productId, int quantity, IPrincipal user)
        {
            this.repository.AddToBasket(productId, quantity, user);
        }

        public void Empty(IPrincipal user)
        {
            this.repository.Empty(user);
        }

        public Basket GetBasketFor(IPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var b = new Basket(user);

            var basketLines = this.repository.GetBasketFor(user);
            foreach (var line in basketLines)
            {
                var basketItem = new Extent(line.Product);
                basketItem.Quantity = line.Quantity;
                b.Contents.Add(basketItem);
            }

            var discountedBasket = this.discountPolicy.Apply(b);
            return discountedBasket;
        }
    }

    public partial class BasketService
    {
        public IEnumerable<Basket> GetAllBaskets()
        {
            foreach (var b in this.repository.GetAllBaskets())
            {
                yield return this.discountPolicy.Apply(b);
            }
        }
    }
}
