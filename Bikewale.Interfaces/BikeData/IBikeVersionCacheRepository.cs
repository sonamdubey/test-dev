using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{

    /// <summary>
    /// Created by  :   Sushil Kumar on 28th June 2016
    /// Description :   Bike Versions  Cache Repository
    /// </summary>
    public interface IBikeVersionCacheRepository<U>
    {
        IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint percentDeviation);
    }
}
