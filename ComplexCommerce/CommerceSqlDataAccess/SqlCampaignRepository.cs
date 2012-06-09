using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public class SqlCampaignRepository : CampaignRepository
    {
        private readonly CommerceObjectContext context;

        public SqlCampaignRepository(string connString)
        {
            if (connString == null)
            {
                throw new ArgumentNullException("connString");
            }

            this.context = new CommerceObjectContext(connString);
        }

        public override IEnumerable<CampaignItem> SelectAll()
        {
            return (from p in this.context.Products
                    select p).AsEnumerable().Select(p => p.ToCampaignItem());
        }

        public override void Update(CampaignItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var dbItem = new Product();
            dbItem.ProductId = item.Product.Id;

            this.context.AttachTo("Products", dbItem);

            dbItem.IsFeatured = item.IsFeatured;
            if (item.DiscountPrice == null)
            {
                dbItem.DiscountedUnitPrice = null;
            }
            else
            {
                dbItem.DiscountedUnitPrice = item.DiscountPrice.Amount;
            }

            this.context.SaveChanges();
        }
    }
}
