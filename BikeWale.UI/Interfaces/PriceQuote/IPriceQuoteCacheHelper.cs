
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    public interface IPriceQuoteCacheHelper
    {
        IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint cityId, uint modelId);
    }
}