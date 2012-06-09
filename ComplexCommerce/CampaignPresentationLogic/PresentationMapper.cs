using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.CampaignPresentation
{
    public class PresentationMapper : IPresentationMapper
    {
        #region IPresentationMapper Members

        public virtual CampaignItemPresenter Map(CampaignItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }        

            var presenter = new CampaignItemPresenter();
            presenter.Id = item.Product.Id;
            presenter.IsFeatured = item.IsFeatured;
            presenter.DiscountPrice = item.DiscountPrice != null ? item.DiscountPrice.Amount : new decimal?();
            presenter.ProductName = item.Product.Name;
            presenter.UnitPrice = item.Product.UnitPrice.Amount;

            return presenter;
        }

        public IEnumerable<CampaignItemPresenter> Map(IEnumerable<CampaignItem> items)
        {
            return from i in items
                   select this.Map(i);
        }

        public CampaignItem Map(CampaignItemPresenter presenter)
        {
            if (presenter == null)
            {
                throw new ArgumentNullException("presenter");
            }
        
            var unitPrice = new Money(presenter.UnitPrice, "DKK");

            Money discountPrice = null;
            if (presenter.DiscountPrice.HasValue)
            {
                discountPrice = new Money(presenter.DiscountPrice.Value, "DKK");
            }

            var product = new Product(presenter.Id, presenter.ProductName, unitPrice);
            return new CampaignItem(product, presenter.IsFeatured, discountPrice);
        }

        #endregion
    }
}
