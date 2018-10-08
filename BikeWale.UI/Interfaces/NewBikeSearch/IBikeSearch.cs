
using Bikewale.ElasticSearch.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Generic;
namespace Bikewale.Interfaces.NewBikeSearch
{
    /// <summary>
    /// Modified By : Prabhu Puredla on 28 sept 2018
    /// Descritption : Added GetBikePriceSearchList
    /// </summary>
    public interface IBikeSearch
    {
        BikeSearchOutputEntity GetBikeSearch(SearchFilters filters);
        IEnumerable<BikeTopVersion> GetBikePriceSearchList(IEnumerable<int> modelIds, uint cityId, BikeSearchEnum source);
    }
}
