using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.Domain;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RepositoryBasketDiscountPolicyAdapter
    {
        private readonly DiscountRepository repository;

        [ImportingConstructor]
        public RepositoryBasketDiscountPolicyAdapter(DiscountRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        [Export]
        public BasketDiscountPolicy RepositoryBasketDiscountPolicy
        {
            get { return new RepositoryBasketDiscountPolicy(this.repository); }
        }
    }
}
