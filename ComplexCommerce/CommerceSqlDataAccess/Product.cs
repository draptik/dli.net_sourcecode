using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public partial class Product
    {
        internal Domain.Product ToDomainProduct()
        {
            return new Domain.Product(this.ProductId, this.Name, new Money(this.UnitPrice, "DKK"));
        }

        internal Domain.Product ToDiscountedProduct()
        {
            return new Domain.Product(this.ProductId, this.Name, new Money(this.DiscountedUnitPrice ?? this.UnitPrice, "DKK"));
        }

        internal Domain.CampaignItem ToCampaignItem()
        {
            Money discountPrice = null;
            if(this.DiscountedUnitPrice != null)
            {
                discountPrice = new Money(this.DiscountedUnitPrice.Value, "DKK");
            }
            return new Domain.CampaignItem(this.ToDomainProduct(), this.IsFeatured, discountPrice);
        }
    }
}
