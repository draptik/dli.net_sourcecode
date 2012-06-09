using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class EvaluatedProduct : IValuableItem<EvaluatedProduct>
    {
        private readonly int id;
        private readonly string name;
        private readonly Money unitPrice;

        public EvaluatedProduct(int id, string name, Money unitPrice)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.id = id;
            this.name = name;
            this.unitPrice = unitPrice;
        }

        public int Id
        {
            get { return this.id; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public Money UnitPrice
        {
            get { return this.unitPrice; }
        }

        public EvaluatedProduct WithUnitPrice(Money unitPrice)
        {
            return new EvaluatedProduct(this.Id, this.Name, unitPrice);
        }

        #region IValuableItem Members

        public Money Value
        {
            get { return this.UnitPrice; }
        }

        public EvaluatedProduct ConvertTo(Currency currency)
        {
            return this.WithUnitPrice(this.UnitPrice.ConvertTo(currency));
        }

        #endregion
    }
}
