using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class RepositoryBasedBasketDiscountPolicy : BasketDiscountPolicy
    {
        private readonly DiscountRepository repository;

        public RepositoryBasedBasketDiscountPolicy(DiscountRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public override Basket Apply(Basket basket)
        {
            var discounts = this.repository.GetDiscountedProducts().ToList();

            var evaluatedBasket = new Basket(basket.Owner);
            foreach (var extent in basket.Contents)
            {
                var evaluatedProduct = (from d in discounts
                                        where d.Id == extent.Item.Id
                                        select d).DefaultIfEmpty(extent.Item).SingleOrDefault();
                evaluatedBasket.Contents.Add(extent.WithItem(evaluatedProduct));
            }

            return evaluatedBasket;
        }
    }
}
