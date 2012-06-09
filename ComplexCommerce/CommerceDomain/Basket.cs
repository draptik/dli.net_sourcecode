using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;

namespace Ploeh.Samples.Commerce.Domain
{
    public class Basket
    {
        private readonly List<Extent> contents;
        private readonly IPrincipal owner;

        public Basket(IPrincipal owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.contents = new List<Extent>();
            this.owner = owner;
        }

        public IList<Extent> Contents
        {
            get { return this.contents; }
        }

        public IPrincipal Owner
        {
            get { return this.owner; }
        }

        public DateTimeOffset Updated
        {
            get { return this.Contents.Select(e => e.Updated).DefaultIfEmpty().Max(); }
        }

        public Basket ConvertTo(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }

            var convertedBasket = new Basket(this.Owner);

            foreach (var extent in this.Contents)
            {
                convertedBasket.Contents.Add(extent.ConvertTo(currency));
            }

            return convertedBasket;
        }
    }
}
