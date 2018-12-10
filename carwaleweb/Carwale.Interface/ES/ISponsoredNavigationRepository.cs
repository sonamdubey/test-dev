using Carwale.Entity.ES;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.ES
{
    public interface ISponsoredNavigationRepository
    {
        List<SponsoredNavigation> GetSponsoredNavigationData(int sectionId, int platformId, out DateTime nextCampaignStartDate);
    }
}
