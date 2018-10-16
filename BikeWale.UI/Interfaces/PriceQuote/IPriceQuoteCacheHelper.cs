
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    public interface IPriceQuoteCacheHelper
    {
        IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(ModelTopVersionPrices modelPrices, uint cityId, uint modelId);
    }
}