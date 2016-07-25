
using BikeWale.Entities.AutoBiz;
using System.Collections.Generic;

namespace Bikewale.Interfaces.AutoBiz
{
    /// <summary>
    /// Modified By :   Sumit Kate on 21 Mar 2015
    /// Description :   New IsSubscribedDealerExists to get dealer for bike version and area
    /// </summary>
    public interface IDealer
    {
        uint IsDealerExists(uint versionId, uint areaId);
        uint IsSubscribedDealerExists(uint versionId, uint areaId);
        //Added By : Sadhana Upadhyay on 26 Oct 2015 To get all dealer dealer price quote details
        IEnumerable<uint> GetAllAvailableDealer(uint versionId, uint areaId);
        IEnumerable<DealerPriceQuoteDetailed> GetDealerPriceQuoteDetail(uint versionId, uint cityId, string dealerIds);
        DealerInfo IsSubscribedDealerExistsV3(uint versionId, uint areaId);
    }
}
