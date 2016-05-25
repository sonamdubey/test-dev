
using System.Collections.Generic;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 20-05-2016
    /// Description : Price Quote Cache method references
    /// </summary>
    public interface IPriceQuoteCache
    {
        IEnumerable<PriceQuoteOfTopCities> FetchPriceQuoteOfTopCitiesCache(uint modelId, uint topCount);
        IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount);
    }
}
