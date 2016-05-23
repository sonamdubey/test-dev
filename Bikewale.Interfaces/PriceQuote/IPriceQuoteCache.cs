
using System.Collections.Generic;

namespace Bikewale.Interfaces.PriceQuote
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 20-05-2016
    /// Description : Price Quote Cache method references
    /// </summary>
    public interface IPriceQuoteCache
    {
        IEnumerable<Bikewale.Entities.PriceQuote.PriceQuoteOfTopCities> FetchPriceQuoteOfTopCitiesCache(uint modelId, uint topCount);
    }
}
