using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.CampaignPresentation
{
    public class CampaignPresenter
    {
        private readonly CampaignRepository repository;
        private readonly IPresentationMapper mapper;

        public CampaignPresenter(CampaignRepository repository,
            IPresentationMapper mapper)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            this.repository = repository;
            this.mapper = mapper;
        }

        public IEnumerable<CampaignItemPresenter> SelectAll()
        {
            return this.mapper.Map(this.repository.SelectAll());
        }

        public void Update(CampaignItemPresenter item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.repository.Update(this.mapper.Map(item));
        }
    }
}
