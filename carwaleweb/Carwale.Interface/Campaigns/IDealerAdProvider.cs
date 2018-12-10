using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.Interfaces.Campaigns
{
    public interface IDealerAdProvider
    {
        DealerAd GetDealerAd(CarIdEntity carEntity, Location locationObj, int platformId);
        DealerAd GetDealerAd(CarIdEntity carEntity, Location locationObj, int platformId, int adType, int campaignId, int PageId);
        List<DealerAd> GetDealerAdList(CarIdEntity carEntity, Location locationObj, int platformId, int adType, int campaignId, int PageId, int count = 0);
    }
}
