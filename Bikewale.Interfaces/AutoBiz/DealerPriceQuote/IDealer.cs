
using BikeWale.Entities.AutoBiz;
using System.Collections.Generic;

namespace Bikewale.Interfaces.AutoBiz
{
    /// <summary>
    /// Modified By :   Sumit Kate on 21 Mar 2015
    /// Description :   New IsSubscribedDealerExists to get dealer for bike version and area
    /// Modified by :   Sumit Kate on 18 Jul 2017
    /// Description :   Add new method GetSubscriptionDealer for primary dealer allocation
    /// </summary>
    public interface IDealer
    {
        uint IsSubscribedDealerExists(uint versionId, uint areaId);
        //Added By : Sadhana Upadhyay on 26 Oct 2015 To get all dealer dealer price quote details        
        IEnumerable<DealerPriceQuoteDetailed> GetDealerPriceQuoteDetail(uint versionId, uint cityId, string dealerIds);
        DealerInfo IsSubscribedDealerExistsV3(uint versionId, uint areaId);
        DealerInfo GetSubscriptionDealer(uint modelId, uint cityId, uint areaId);
    }
}
