using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.Domain;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BasketServiceAdapter
    {
        private readonly BasketRepository repository;
        private readonly BasketDiscountPolicy discountPolicy;

        [ImportingConstructor]
        public BasketServiceAdapter(BasketRepository repository, BasketDiscountPolicy discountPolicy)
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

        [Export]
        public IBasketService BasketService
        {
            get { return new BasketService(this.repository, this.discountPolicy); }
        }
    }
}
