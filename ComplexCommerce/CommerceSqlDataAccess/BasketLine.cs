using Ploeh.Samples.Commerce.Domain;
using System;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public partial class BasketLine
    {
        internal Extent ToDomainProductExtent()
        {
            var product = this.Product.ToDomainProduct();
            var pe = new Extent(product);
            pe.Quantity = this.Quantity;
            pe.Updated = new DateTimeOffset(DateTime.SpecifyKind(this.UtcUpdated, DateTimeKind.Utc));

            return pe;
        }
    }
}
