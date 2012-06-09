using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.CampaignPresentation;

namespace Ploeh.Samples.Commerce.Campaign.WebForms
{
    public class CampaignDataSource
    {
        private readonly CampaignPresenter presenter;

        public CampaignDataSource()
        {
            var container = 
                (CampaignContainer)HttpContext.Current
                .Application["container"];
            this.presenter = container.ResolvePresenter();
        }

        public IEnumerable<CampaignItemPresenter> SelectAll()
        {
            return this.presenter.SelectAll();
        }

        public void Update(CampaignItemPresenter item)
        {
            this.presenter.Update(item);
        }
    }
}
