

using BikeWale.Entities.AutoBiz;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Modofier    :Kartik Rathod on 28 sept 2018
    /// Desc        : added GetNearestDealer(uint modelId, uint cityId)
    /// </summary>
    public interface IDealerPriceQuoteCache
    {
        IEnumerable<PQ_VersionPrice> GetDealerPriceQuotesByModelCity(uint cityId, uint modelId, uint dealerId);
		IEnumerable<string> GetMLAMakeCities();
        IEnumerable<uint> GetNearestDealer(uint modelId, uint cityId);
    }
}
