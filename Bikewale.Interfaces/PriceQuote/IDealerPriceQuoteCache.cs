

using BikeWale.Entities.AutoBiz;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    public interface IDealerPriceQuoteCache
    {
        IEnumerable<PQ_VersionPrice> GetDealerPriceQuotesByModelCity(uint cityId, uint modelId, uint dealerId);
    }
}
