using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.CampaignPresentation;
using System.Configuration;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.Commerce.Campaign.WebForms
{
    public class CampaignContainer
    {
        public CampaignPresenter ResolvePresenter()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;
            CampaignRepository repository =
                new SqlCampaignRepository(connectionString);

            IPresentationMapper mapper = 
                new PresentationMapper();

            return new CampaignPresenter(repository, mapper);
        }
    }
}
