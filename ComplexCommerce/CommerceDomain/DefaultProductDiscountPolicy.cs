using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class DefaultProductDiscountPolicy : BasketDiscountPolicy
    {
        public override Basket Apply(Basket basket)
        {
            var policy = new DefaultCustomerDiscountPolicy();

            var evaluatedBasket = new Basket(basket.Owner);
            foreach (var extent in basket.Contents)
            {
                var evaluatedProduct = policy.Apply(extent.Product, basket.Owner);
                evaluatedBasket.Contents.Add(extent.WithItem(evaluatedProduct));
            }

            return evaluatedBasket;
        }
    }
}
