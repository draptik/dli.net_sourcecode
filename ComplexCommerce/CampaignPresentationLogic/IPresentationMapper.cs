using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.CampaignPresentation
{
    public interface IPresentationMapper
    {
        CampaignItemPresenter Map(CampaignItem item);

        IEnumerable<CampaignItemPresenter> Map(IEnumerable<CampaignItem> items);

        CampaignItem Map(CampaignItemPresenter presenter);
    }
}
