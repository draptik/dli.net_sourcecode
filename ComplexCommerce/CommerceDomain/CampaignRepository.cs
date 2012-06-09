using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public abstract class CampaignRepository
    {
        public abstract IEnumerable<CampaignItem> SelectAll();

        public abstract void Update(CampaignItem item);
    }
}
