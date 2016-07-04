using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{

    /// <summary>
    /// Created by  :   Sushil Kumar on 28th June 2016
    /// Description :   Bike Versions  Cache Repository
    /// </summary>
    public interface IBikeVersionCacheRepository<T, U>
    {
        IEnumerable<SimilarBikeEntity> GetSimilarBikesList(U versionId, uint topCount, uint percentDeviation);
        List<BikeVersionsListEntity> GetVersionsByType(EnumBikeType requestType, int modelId, int? cityId = null);
        T GetById(U versionId);
        List<BikeVersionMinSpecs> GetVersionMinSpecs(uint modelId, bool isNew);
    }
}
